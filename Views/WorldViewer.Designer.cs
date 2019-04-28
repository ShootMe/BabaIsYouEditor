﻿namespace BabaIsYou.Views {
	partial class WorldViewer {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.listLevels = new BabaIsYou.Controls.ListPanel();
			this.txtLevelFilter = new System.Windows.Forms.TextBox();
			this.splitObjectsLevel = new System.Windows.Forms.SplitContainer();
			this.listObjects = new BabaIsYou.Controls.ListPanel();
			this.mapViewer = new BabaIsYou.Controls.MapViewer();
			this.menu = new System.Windows.Forms.MenuStrip();
			this.menuWorld = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemAddWorld = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemOpenWorld = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSaveWorld = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWorldSperator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemWorldProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWorldSeperator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemAddLevel = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemRemoveLevel = new System.Windows.Forms.ToolStripMenuItem();
			this.menuLevel = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLevelProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSetTheme = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemNewTheme = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemThemeSeperator = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemThemeNone = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLevelSeperator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemShowStacked = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemShowDirections = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLevelSeperator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemRevertChanges = new System.Windows.Forms.ToolStripMenuItem();
			this.menuPalette = new System.Windows.Forms.ToolStripMenuItem();
			this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemReadMe = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWebsite = new System.Windows.Forms.ToolStripMenuItem();
			this.status = new System.Windows.Forms.StatusStrip();
			this.statusLevel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusLevelName = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusLevelInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusBlank = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSprite = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusPosition = new System.Windows.Forms.ToolStripStatusLabel();
			this.imgBaba = new System.Windows.Forms.PictureBox();
			this.menuItemEdgePlacement = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitObjectsLevel)).BeginInit();
			this.splitObjectsLevel.Panel1.SuspendLayout();
			this.splitObjectsLevel.Panel2.SuspendLayout();
			this.splitObjectsLevel.SuspendLayout();
			this.menu.SuspendLayout();
			this.status.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgBaba)).BeginInit();
			this.SuspendLayout();
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitMain.ForeColor = System.Drawing.Color.White;
			this.splitMain.IsSplitterFixed = true;
			this.splitMain.Location = new System.Drawing.Point(0, 24);
			this.splitMain.Margin = new System.Windows.Forms.Padding(0);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.listLevels);
			this.splitMain.Panel1.Controls.Add(this.txtLevelFilter);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.splitObjectsLevel);
			this.splitMain.Size = new System.Drawing.Size(1009, 778);
			this.splitMain.SplitterDistance = 144;
			this.splitMain.SplitterWidth = 1;
			this.splitMain.TabIndex = 1;
			// 
			// listLevels
			// 
			this.listLevels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listLevels.Focusable = false;
			this.listLevels.Location = new System.Drawing.Point(0, 20);
			this.listLevels.Name = "listLevels";
			this.listLevels.Size = new System.Drawing.Size(144, 758);
			this.listLevels.TabIndex = 0;
			this.listLevels.TabStop = false;
			this.listLevels.IndexChanged += new BabaIsYou.Controls.ListPanel.SelectedIndexChangedEvent(this.listLevels_IndexChanged);
			this.listLevels.ItemClicked += new BabaIsYou.Controls.ListPanel.ItemClickedEvent(this.listLevels_ItemClicked);
			// 
			// txtLevelFilter
			// 
			this.txtLevelFilter.BackColor = System.Drawing.Color.White;
			this.txtLevelFilter.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtLevelFilter.ForeColor = System.Drawing.Color.Gray;
			this.txtLevelFilter.Location = new System.Drawing.Point(0, 0);
			this.txtLevelFilter.Name = "txtLevelFilter";
			this.txtLevelFilter.Size = new System.Drawing.Size(144, 20);
			this.txtLevelFilter.TabIndex = 3;
			this.txtLevelFilter.Text = "Filter ...";
			this.txtLevelFilter.TextChanged += new System.EventHandler(this.txtLevelFilter_TextChanged);
			this.txtLevelFilter.Enter += new System.EventHandler(this.txtLevelFilter_Enter);
			this.txtLevelFilter.Leave += new System.EventHandler(this.txtLevelFilter_Leave);
			// 
			// splitObjectsLevel
			// 
			this.splitObjectsLevel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitObjectsLevel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitObjectsLevel.IsSplitterFixed = true;
			this.splitObjectsLevel.Location = new System.Drawing.Point(0, 0);
			this.splitObjectsLevel.Name = "splitObjectsLevel";
			this.splitObjectsLevel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitObjectsLevel.Panel1
			// 
			this.splitObjectsLevel.Panel1.Controls.Add(this.listObjects);
			// 
			// splitObjectsLevel.Panel2
			// 
			this.splitObjectsLevel.Panel2.Controls.Add(this.mapViewer);
			this.splitObjectsLevel.Size = new System.Drawing.Size(864, 778);
			this.splitObjectsLevel.SplitterDistance = 180;
			this.splitObjectsLevel.SplitterWidth = 1;
			this.splitObjectsLevel.TabIndex = 1;
			// 
			// listObjects
			// 
			this.listObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listObjects.DrawText = false;
			this.listObjects.Location = new System.Drawing.Point(0, 0);
			this.listObjects.MaximumSize = new System.Drawing.Size(864, 180);
			this.listObjects.Name = "listObjects";
			this.listObjects.Size = new System.Drawing.Size(864, 180);
			this.listObjects.TabIndex = 0;
			this.listObjects.IndexChanged += new BabaIsYou.Controls.ListPanel.SelectedIndexChangedEvent(this.listObjects_IndexChanged);
			this.listObjects.ItemClicked += new BabaIsYou.Controls.ListPanel.ItemClickedEvent(this.listObjects_ItemClicked);
			// 
			// mapViewer
			// 
			this.mapViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapViewer.Location = new System.Drawing.Point(0, 0);
			this.mapViewer.Name = "mapViewer";
			this.mapViewer.Size = new System.Drawing.Size(864, 597);
			this.mapViewer.TabIndex = 0;
			this.mapViewer.TabStop = false;
			this.mapViewer.CellMouseOver += new BabaIsYou.Controls.MapViewer.CellMouseEvent(this.mapViewer_CellMouseOver);
			this.mapViewer.CellMouseDown += new BabaIsYou.Controls.MapViewer.CellMouseEvent(this.mapViewer_CellMouseDown);
			this.mapViewer.CellMouseWheel += new BabaIsYou.Controls.MapViewer.CellMouseEvent(this.mapViewer_CellMouseWheel);
			this.mapViewer.DrawCurrentCellStart += new BabaIsYou.Controls.MapViewer.DrawCurrentCellEvent(this.mapViewer_DrawCurrentCellStart);
			this.mapViewer.DrawCurrentCellFinish += new BabaIsYou.Controls.MapViewer.DrawCurrentCellEvent(this.mapViewer_DrawCurrentCellFinish);
			this.mapViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapViewer_MouseUp);
			// 
			// menu
			// 
			this.menu.BackColor = System.Drawing.SystemColors.Control;
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWorld,
            this.menuLevel,
            this.menuPalette,
            this.menuHelp});
			this.menu.Location = new System.Drawing.Point(0, 0);
			this.menu.Name = "menu";
			this.menu.Size = new System.Drawing.Size(1009, 24);
			this.menu.TabIndex = 1;
			// 
			// menuWorld
			// 
			this.menuWorld.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddWorld,
            this.menuItemOpenWorld,
            this.menuItemSaveWorld,
            this.menuItemWorldSperator1,
            this.menuItemWorldProperties,
            this.menuItemWorldSeperator2,
            this.menuItemAddLevel,
            this.menuItemRemoveLevel});
			this.menuWorld.Name = "menuWorld";
			this.menuWorld.Size = new System.Drawing.Size(51, 20);
			this.menuWorld.Text = "World";
			// 
			// menuItemAddWorld
			// 
			this.menuItemAddWorld.Image = global::BabaIsYou.Properties.Resources.add;
			this.menuItemAddWorld.Name = "menuItemAddWorld";
			this.menuItemAddWorld.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
			this.menuItemAddWorld.Size = new System.Drawing.Size(188, 22);
			this.menuItemAddWorld.Text = "Add World";
			this.menuItemAddWorld.Click += new System.EventHandler(this.menuItemAddWorld_Click);
			// 
			// menuItemOpenWorld
			// 
			this.menuItemOpenWorld.Image = global::BabaIsYou.Properties.Resources.open;
			this.menuItemOpenWorld.Name = "menuItemOpenWorld";
			this.menuItemOpenWorld.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.menuItemOpenWorld.Size = new System.Drawing.Size(188, 22);
			this.menuItemOpenWorld.Text = "Open World";
			this.menuItemOpenWorld.Click += new System.EventHandler(this.menuItemOpenWorld_Click);
			// 
			// menuItemSaveWorld
			// 
			this.menuItemSaveWorld.Image = global::BabaIsYou.Properties.Resources.save;
			this.menuItemSaveWorld.Name = "menuItemSaveWorld";
			this.menuItemSaveWorld.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.menuItemSaveWorld.Size = new System.Drawing.Size(188, 22);
			this.menuItemSaveWorld.Text = "Save World";
			this.menuItemSaveWorld.Click += new System.EventHandler(this.menuItemSaveWorld_Click);
			// 
			// menuItemWorldSperator1
			// 
			this.menuItemWorldSperator1.Name = "menuItemWorldSperator1";
			this.menuItemWorldSperator1.Size = new System.Drawing.Size(185, 6);
			// 
			// menuItemWorldProperties
			// 
			this.menuItemWorldProperties.Image = global::BabaIsYou.Properties.Resources.config;
			this.menuItemWorldProperties.Name = "menuItemWorldProperties";
			this.menuItemWorldProperties.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.menuItemWorldProperties.Size = new System.Drawing.Size(188, 22);
			this.menuItemWorldProperties.Text = "Properties";
			this.menuItemWorldProperties.Click += new System.EventHandler(this.menuItemWorldProperties_Click);
			// 
			// menuItemWorldSeperator2
			// 
			this.menuItemWorldSeperator2.Name = "menuItemWorldSeperator2";
			this.menuItemWorldSeperator2.Size = new System.Drawing.Size(185, 6);
			// 
			// menuItemAddLevel
			// 
			this.menuItemAddLevel.Image = global::BabaIsYou.Properties.Resources.add;
			this.menuItemAddLevel.Name = "menuItemAddLevel";
			this.menuItemAddLevel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.menuItemAddLevel.Size = new System.Drawing.Size(188, 22);
			this.menuItemAddLevel.Text = "Add Level";
			this.menuItemAddLevel.Click += new System.EventHandler(this.menuItemAddLevel_Click);
			// 
			// menuItemRemoveLevel
			// 
			this.menuItemRemoveLevel.Image = global::BabaIsYou.Properties.Resources.delete;
			this.menuItemRemoveLevel.Name = "menuItemRemoveLevel";
			this.menuItemRemoveLevel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.menuItemRemoveLevel.Size = new System.Drawing.Size(188, 22);
			this.menuItemRemoveLevel.Text = "Remove Level";
			this.menuItemRemoveLevel.Click += new System.EventHandler(this.menuItemRemoveLevel_Click);
			// 
			// menuLevel
			// 
			this.menuLevel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLevelProperties,
            this.menuItemSetTheme,
            this.menuItemLevelSeperator1,
            this.menuItemShowStacked,
            this.menuItemShowDirections,
            this.menuItemEdgePlacement,
            this.menuItemLevelSeperator2,
            this.menuItemRevertChanges});
			this.menuLevel.Name = "menuLevel";
			this.menuLevel.Size = new System.Drawing.Size(46, 20);
			this.menuLevel.Text = "Level";
			// 
			// menuItemLevelProperties
			// 
			this.menuItemLevelProperties.Image = global::BabaIsYou.Properties.Resources.config;
			this.menuItemLevelProperties.Name = "menuItemLevelProperties";
			this.menuItemLevelProperties.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.menuItemLevelProperties.Size = new System.Drawing.Size(201, 22);
			this.menuItemLevelProperties.Text = "Properties";
			this.menuItemLevelProperties.Click += new System.EventHandler(this.menuItemLevelProperties_Click);
			// 
			// menuItemSetTheme
			// 
			this.menuItemSetTheme.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNewTheme,
            this.menuItemThemeSeperator,
            this.menuItemThemeNone});
			this.menuItemSetTheme.Image = global::BabaIsYou.Properties.Resources.theme;
			this.menuItemSetTheme.Name = "menuItemSetTheme";
			this.menuItemSetTheme.Size = new System.Drawing.Size(201, 22);
			this.menuItemSetTheme.Text = "Set Theme";
			this.menuItemSetTheme.DropDownOpening += new System.EventHandler(this.menuItemSetTheme_DropDownOpening);
			// 
			// menuItemNewTheme
			// 
			this.menuItemNewTheme.Image = global::BabaIsYou.Properties.Resources.add;
			this.menuItemNewTheme.Name = "menuItemNewTheme";
			this.menuItemNewTheme.Size = new System.Drawing.Size(138, 22);
			this.menuItemNewTheme.Text = "New Theme";
			this.menuItemNewTheme.Click += new System.EventHandler(this.menuItemNewTheme_Click);
			// 
			// menuItemThemeSeperator
			// 
			this.menuItemThemeSeperator.Name = "menuItemThemeSeperator";
			this.menuItemThemeSeperator.Size = new System.Drawing.Size(135, 6);
			// 
			// menuItemThemeNone
			// 
			this.menuItemThemeNone.Image = global::BabaIsYou.Properties.Resources.delete;
			this.menuItemThemeNone.Name = "menuItemThemeNone";
			this.menuItemThemeNone.Size = new System.Drawing.Size(138, 22);
			this.menuItemThemeNone.Text = "none";
			this.menuItemThemeNone.Click += new System.EventHandler(this.menuItemSetTheme_Click);
			// 
			// menuItemLevelSeperator1
			// 
			this.menuItemLevelSeperator1.Name = "menuItemLevelSeperator1";
			this.menuItemLevelSeperator1.Size = new System.Drawing.Size(198, 6);
			// 
			// menuItemShowStacked
			// 
			this.menuItemShowStacked.CheckOnClick = true;
			this.menuItemShowStacked.Name = "menuItemShowStacked";
			this.menuItemShowStacked.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
			this.menuItemShowStacked.Size = new System.Drawing.Size(201, 22);
			this.menuItemShowStacked.Text = "Show Stacked #";
			this.menuItemShowStacked.Click += new System.EventHandler(this.menuItemShowStacked_Click);
			// 
			// menuItemShowDirections
			// 
			this.menuItemShowDirections.CheckOnClick = true;
			this.menuItemShowDirections.Name = "menuItemShowDirections";
			this.menuItemShowDirections.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.menuItemShowDirections.Size = new System.Drawing.Size(201, 22);
			this.menuItemShowDirections.Text = "Show Directions";
			this.menuItemShowDirections.Click += new System.EventHandler(this.menuItemShowDirections_Click);
			// 
			// menuItemLevelSeperator2
			// 
			this.menuItemLevelSeperator2.Name = "menuItemLevelSeperator2";
			this.menuItemLevelSeperator2.Size = new System.Drawing.Size(198, 6);
			// 
			// menuItemRevertChanges
			// 
			this.menuItemRevertChanges.Image = global::BabaIsYou.Properties.Resources.undo;
			this.menuItemRevertChanges.Name = "menuItemRevertChanges";
			this.menuItemRevertChanges.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.menuItemRevertChanges.Size = new System.Drawing.Size(201, 22);
			this.menuItemRevertChanges.Text = "Revert Changes";
			this.menuItemRevertChanges.Click += new System.EventHandler(this.menuItemRevertChanges_Click);
			// 
			// menuPalette
			// 
			this.menuPalette.Name = "menuPalette";
			this.menuPalette.Size = new System.Drawing.Size(55, 20);
			this.menuPalette.Text = "Palette";
			// 
			// menuHelp
			// 
			this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReadMe,
            this.menuItemWebsite});
			this.menuHelp.Name = "menuHelp";
			this.menuHelp.Size = new System.Drawing.Size(44, 20);
			this.menuHelp.Text = "Help";
			// 
			// menuItemReadMe
			// 
			this.menuItemReadMe.Image = global::BabaIsYou.Properties.Resources.info;
			this.menuItemReadMe.Name = "menuItemReadMe";
			this.menuItemReadMe.Size = new System.Drawing.Size(120, 22);
			this.menuItemReadMe.Text = "Read Me";
			this.menuItemReadMe.Click += new System.EventHandler(this.menuItemReadMe_Click);
			// 
			// menuItemWebsite
			// 
			this.menuItemWebsite.Image = global::BabaIsYou.Properties.Resources.web;
			this.menuItemWebsite.Name = "menuItemWebsite";
			this.menuItemWebsite.Size = new System.Drawing.Size(120, 22);
			this.menuItemWebsite.Text = "Website";
			this.menuItemWebsite.Click += new System.EventHandler(this.menuItemWebsite_Click);
			// 
			// status
			// 
			this.status.AllowMerge = false;
			this.status.BackColor = System.Drawing.SystemColors.Control;
			this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLevel,
            this.statusLevelName,
            this.statusLevelInfo,
            this.statusBlank,
            this.statusSprite,
            this.statusPosition});
			this.status.Location = new System.Drawing.Point(0, 802);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(1009, 24);
			this.status.TabIndex = 2;
			// 
			// statusLevel
			// 
			this.statusLevel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statusLevel.Name = "statusLevel";
			this.statusLevel.Size = new System.Drawing.Size(56, 19);
			this.statusLevel.Text = "LevelFile";
			// 
			// statusLevelName
			// 
			this.statusLevelName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statusLevelName.Name = "statusLevelName";
			this.statusLevelName.Size = new System.Drawing.Size(70, 19);
			this.statusLevelName.Text = "LevelName";
			// 
			// statusLevelInfo
			// 
			this.statusLevelInfo.Name = "statusLevelInfo";
			this.statusLevelInfo.Size = new System.Drawing.Size(55, 19);
			this.statusLevelInfo.Text = "LevelInfo";
			// 
			// statusBlank
			// 
			this.statusBlank.Name = "statusBlank";
			this.statusBlank.Size = new System.Drawing.Size(737, 19);
			this.statusBlank.Spring = true;
			// 
			// statusSprite
			// 
			this.statusSprite.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statusSprite.Name = "statusSprite";
			this.statusSprite.Size = new System.Drawing.Size(41, 19);
			this.statusSprite.Text = "Sprite";
			// 
			// statusPosition
			// 
			this.statusPosition.Name = "statusPosition";
			this.statusPosition.Size = new System.Drawing.Size(35, 19);
			this.statusPosition.Text = "[X, Y]";
			// 
			// imgBaba
			// 
			this.imgBaba.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imgBaba.Image = global::BabaIsYou.Properties.Resources.baba;
			this.imgBaba.Location = new System.Drawing.Point(0, 24);
			this.imgBaba.Name = "imgBaba";
			this.imgBaba.Size = new System.Drawing.Size(1009, 778);
			this.imgBaba.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.imgBaba.TabIndex = 1;
			this.imgBaba.TabStop = false;
			this.imgBaba.Visible = false;
			// 
			// menuItemEdgePlacement
			// 
			this.menuItemEdgePlacement.CheckOnClick = true;
			this.menuItemEdgePlacement.Name = "menuItemEdgePlacement";
			this.menuItemEdgePlacement.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.menuItemEdgePlacement.Size = new System.Drawing.Size(201, 22);
			this.menuItemEdgePlacement.Text = "Edge Placement";
			this.menuItemEdgePlacement.Click += new System.EventHandler(this.menuItemEdgePlacement_Click);
			// 
			// WorldViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(1009, 826);
			this.Controls.Add(this.imgBaba);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.status);
			this.Controls.Add(this.menu);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menu;
			this.Name = "WorldViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Baba Is You";
			this.Deactivate += new System.EventHandler(this.WorldViewer_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorldViewer_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WorldViewer_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WorldViewer_KeyUp);
			this.Resize += new System.EventHandler(this.Solver_Resize);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel1.PerformLayout();
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.splitObjectsLevel.Panel1.ResumeLayout(false);
			this.splitObjectsLevel.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitObjectsLevel)).EndInit();
			this.splitObjectsLevel.ResumeLayout(false);
			this.menu.ResumeLayout(false);
			this.menu.PerformLayout();
			this.status.ResumeLayout(false);
			this.status.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgBaba)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.SplitContainer splitMain;
		private Controls.ListPanel listLevels;
		private Controls.ListPanel listObjects;
		private System.Windows.Forms.SplitContainer splitObjectsLevel;
		private System.Windows.Forms.MenuStrip menu;
		private System.Windows.Forms.ToolStripMenuItem menuWorld;
		private System.Windows.Forms.ToolStripMenuItem menuItemSaveWorld;
		private System.Windows.Forms.StatusStrip status;
		private System.Windows.Forms.ToolStripStatusLabel statusPosition;
		private System.Windows.Forms.ToolStripStatusLabel statusSprite;
		private System.Windows.Forms.ToolStripStatusLabel statusLevel;
		private System.Windows.Forms.ToolStripStatusLabel statusBlank;
		private System.Windows.Forms.ToolStripStatusLabel statusLevelName;
		private System.Windows.Forms.ToolStripStatusLabel statusLevelInfo;
		private Controls.MapViewer mapViewer;
		private System.Windows.Forms.ToolStripMenuItem menuItemOpenWorld;
		private System.Windows.Forms.ToolStripMenuItem menuPalette;
		private System.Windows.Forms.ToolStripMenuItem menuLevel;
		private System.Windows.Forms.ToolStripMenuItem menuItemLevelProperties;
		private System.Windows.Forms.ToolStripSeparator menuItemLevelSeperator1;
		private System.Windows.Forms.ToolStripMenuItem menuItemRevertChanges;
		private System.Windows.Forms.PictureBox imgBaba;
		private System.Windows.Forms.TextBox txtLevelFilter;
		private System.Windows.Forms.ToolStripSeparator menuItemWorldSperator1;
		private System.Windows.Forms.ToolStripMenuItem menuItemAddLevel;
		private System.Windows.Forms.ToolStripMenuItem menuItemRemoveLevel;
		private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private System.Windows.Forms.ToolStripMenuItem menuItemReadMe;
		private System.Windows.Forms.ToolStripMenuItem menuItemWebsite;
		private System.Windows.Forms.ToolStripMenuItem menuItemWorldProperties;
		private System.Windows.Forms.ToolStripSeparator menuItemWorldSeperator2;
		private System.Windows.Forms.ToolStripMenuItem menuItemShowStacked;
		private System.Windows.Forms.ToolStripSeparator menuItemLevelSeperator2;
		private System.Windows.Forms.ToolStripMenuItem menuItemAddWorld;
		private System.Windows.Forms.ToolStripMenuItem menuItemShowDirections;
		private System.Windows.Forms.ToolStripMenuItem menuItemSetTheme;
		private System.Windows.Forms.ToolStripMenuItem menuItemThemeNone;
		private System.Windows.Forms.ToolStripMenuItem menuItemNewTheme;
		private System.Windows.Forms.ToolStripSeparator menuItemThemeSeperator;
		private System.Windows.Forms.ToolStripMenuItem menuItemEdgePlacement;
	}
}

