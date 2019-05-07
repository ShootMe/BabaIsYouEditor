using System;
using System.Reflection;
using System.Windows.Forms;
namespace BabaIsYou.Controls {
	public class OpenFolderDialog {
		private string _initialDirectory;
		private string title;

		public string InitialDirectory {
			get { return string.IsNullOrEmpty(_initialDirectory) ? Environment.CurrentDirectory : _initialDirectory; }
			set { _initialDirectory = value; }
		}
		public string Title {
			get { return title ?? "Select a folder"; }
			set { title = value; }
		}
		public string FileName { get; set; }

		public bool Show() { return Show(IntPtr.Zero); }

		public bool Show(IntPtr hWndOwner) {
			var result = Environment.OSVersion.Version.Major >= 6
				? WindowsDialog.Show(hWndOwner, InitialDirectory, Title)
				: ShowXpDialog(hWndOwner, InitialDirectory, Title);
			FileName = result.FileName;
			return result.Result;
		}

		private struct ShowDialogResult {
			public bool Result { get; set; }
			public string FileName { get; set; }
		}

		private static ShowDialogResult ShowXpDialog(IntPtr ownerHandle, string initialDirectory, string title) {
			var folderBrowserDialog = new FolderBrowserDialog {
				Description = title,
				SelectedPath = initialDirectory,
				ShowNewFolderButton = false
			};
			var dialogResult = new ShowDialogResult();
			if (folderBrowserDialog.ShowDialog(new WindowWrapper(ownerHandle)) == DialogResult.OK) {
				dialogResult.Result = true;
				dialogResult.FileName = folderBrowserDialog.SelectedPath;
			}
			return dialogResult;
		}

		private static class WindowsDialog {
			private const string foldersFilter = "Folders|\n";
			private const BindingFlags c_flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			private readonly static Assembly windowsFormsAssembly = typeof(FileDialog).Assembly;
			private readonly static Type iFileDialogType = windowsFormsAssembly.GetType("System.Windows.Forms.FileDialogNative+IFileDialog");
			private readonly static MethodInfo createVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("CreateVistaDialog", c_flags);
			private readonly static MethodInfo onBeforeVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("OnBeforeVistaDialog", c_flags);
			private readonly static MethodInfo getOptionsMethodInfo = typeof(FileDialog).GetMethod("GetOptions", c_flags);
			private readonly static MethodInfo setOptionsMethodInfo = iFileDialogType.GetMethod("SetOptions", c_flags);
			private readonly static uint fosPickFoldersBitFlag = (uint)windowsFormsAssembly.GetType("System.Windows.Forms.FileDialogNative+FOS").GetField("FOS_PICKFOLDERS").GetValue(null);
			private readonly static ConstructorInfo vistaDialogEventsConstructorInfo = windowsFormsAssembly.GetType("System.Windows.Forms.FileDialog+VistaDialogEvents").GetConstructor(c_flags, null, new[] { typeof(FileDialog) }, null);
			private readonly static MethodInfo adviseMethodInfo = iFileDialogType.GetMethod("Advise");
			private readonly static MethodInfo unAdviseMethodInfo = iFileDialogType.GetMethod("Unadvise");
			private readonly static MethodInfo showMethodInfo = iFileDialogType.GetMethod("Show");

			public static ShowDialogResult Show(IntPtr ownerHandle, string initialDirectory, string title) {
				using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
					openFileDialog.AddExtension = false;
					openFileDialog.CheckFileExists = false;
					openFileDialog.DereferenceLinks = true;
					openFileDialog.Filter = foldersFilter;
					openFileDialog.InitialDirectory = initialDirectory;
					openFileDialog.Multiselect = false;
					openFileDialog.Title = title;
					openFileDialog.RestoreDirectory = true;

					var iFileDialog = createVistaDialogMethodInfo.Invoke(openFileDialog, new object[] { });
					onBeforeVistaDialogMethodInfo.Invoke(openFileDialog, new[] { iFileDialog });
					setOptionsMethodInfo.Invoke(iFileDialog, new object[] { (uint)getOptionsMethodInfo.Invoke(openFileDialog, new object[] { }) | fosPickFoldersBitFlag });
					var adviseParametersWithOutputConnectionToken = new[] { vistaDialogEventsConstructorInfo.Invoke(new object[] { openFileDialog }), 0U };
					adviseMethodInfo.Invoke(iFileDialog, adviseParametersWithOutputConnectionToken);
					
					try {
						int retVal = (int)showMethodInfo.Invoke(iFileDialog, new object[] { ownerHandle });
						return new ShowDialogResult {
							Result = retVal == 0,
							FileName = openFileDialog.FileName
						};
					} finally {
						unAdviseMethodInfo.Invoke(iFileDialog, new[] { adviseParametersWithOutputConnectionToken[1] });
					}
				}
			}
		}

		private class WindowWrapper : IWin32Window {
			public WindowWrapper(IntPtr handle) {
				Handle = handle;
			}
			public IntPtr Handle { get; }
		}
	}
}