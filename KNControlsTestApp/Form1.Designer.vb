<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.StarsField = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.EditableCellTable = New KNControls.KNTableView()
        Me.KnTokenField1 = New KNControls.KNTokenField()
        Me.KnLeopardStyleHeaderButton1 = New KNControls.KNLeopardStyleHeaderButton()
        Me.KnRatingField1 = New KNControls.KNRatingField()
        Me.KnTableView1 = New KNControls.KNTableView()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LickToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.KnProgressWheel1 = New KNControls.KNProgressWheel()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StarsField
        '
        Me.StarsField.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.StarsField.AutoSize = True
        Me.StarsField.Location = New System.Drawing.Point(416, 327)
        Me.StarsField.Name = "StarsField"
        Me.StarsField.Size = New System.Drawing.Size(61, 13)
        Me.StarsField.TabIndex = 3
        Me.StarsField.Text = "Some Stars"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(12, 326)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(42, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Add"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(60, 326)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(59, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Remove"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button3.Location = New System.Drawing.Point(125, 326)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(59, 23)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Post"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(483, 324)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(12, 288)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(516, 23)
        Me.Label1.TabIndex = 8
        '
        'EditableCellTable
        '
        Me.EditableCellTable.AllowEmptySelection = True
        Me.EditableCellTable.AllowsMultipleSelection = True
        Me.EditableCellTable.AlternateBackgroundColour = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.EditableCellTable.AlternatingRowBackgrounds = True
        Me.EditableCellTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditableCellTable.AutoHideScrollBars = False
        Me.EditableCellTable.BackgroundColour = System.Drawing.Color.White
        Me.EditableCellTable.CornerCell = Nothing
        Me.EditableCellTable.DataSource = Nothing
        Me.EditableCellTable.DefaultRowHeight = 20
        Me.EditableCellTable.DrawHorizontalGridLines = False
        Me.EditableCellTable.DrawVerticalGridLines = False
        Me.EditableCellTable.GridColour = System.Drawing.Color.LightGray
        Me.EditableCellTable.HeaderHeight = 20
        Me.EditableCellTable.HeaderSizingMode = KNControls.KNTableView.HeaderAutoSizing.DivideEqually
        Me.EditableCellTable.Location = New System.Drawing.Point(294, 27)
        Me.EditableCellTable.Name = "EditableCellTable"
        Me.EditableCellTable.SelectedRows = CType(resources.GetObject("EditableCellTable.SelectedRows"), System.Collections.ArrayList)
        Me.EditableCellTable.SelectionStyling = KNControls.KNTableView.SelectionStyle.FlatStyle
        Me.EditableCellTable.Size = New System.Drawing.Size(289, 232)
        Me.EditableCellTable.TabIndex = 10
        Me.EditableCellTable.TableDelegate = Nothing
        '
        'KnTokenField1
        '
        Me.KnTokenField1.AllowDrop = True
        Me.KnTokenField1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KnTokenField1.Editable = True
        Me.KnTokenField1.Items = New Object(-1) {}
        Me.KnTokenField1.Location = New System.Drawing.Point(12, 265)
        Me.KnTokenField1.Name = "KnTokenField1"
        Me.KnTokenField1.Size = New System.Drawing.Size(571, 20)
        Me.KnTokenField1.TabIndex = 7
        '
        'KnLeopardStyleHeaderButton1
        '
        Me.KnLeopardStyleHeaderButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.KnLeopardStyleHeaderButton1.Borders = 15
        Me.KnLeopardStyleHeaderButton1.CustomEndColor = System.Drawing.Color.Empty
        Me.KnLeopardStyleHeaderButton1.CustomStartColor = System.Drawing.Color.Empty
        Me.KnLeopardStyleHeaderButton1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.KnLeopardStyleHeaderButton1.Image = CType(resources.GetObject("KnLeopardStyleHeaderButton1.Image"), System.Drawing.Bitmap)
        Me.KnLeopardStyleHeaderButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.KnLeopardStyleHeaderButton1.Location = New System.Drawing.Point(190, 314)
        Me.KnLeopardStyleHeaderButton1.Name = "KnLeopardStyleHeaderButton1"
        Me.KnLeopardStyleHeaderButton1.Size = New System.Drawing.Size(140, 35)
        Me.KnLeopardStyleHeaderButton1.Style = KNControls.KNLeopardStyleHeaderButton.ButtonStyleEnum.Leopard
        Me.KnLeopardStyleHeaderButton1.TabIndex = 6
        Me.KnLeopardStyleHeaderButton1.Text = "Mac Style Button"
        Me.KnLeopardStyleHeaderButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'KnRatingField1
        '
        Me.KnRatingField1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.KnRatingField1.BackgroundColor = System.Drawing.Color.White
        Me.KnRatingField1.BorderColor = System.Drawing.Color.LightGray
        Me.KnRatingField1.DrawBorder = True
        Me.KnRatingField1.Editable = True
        Me.KnRatingField1.FillBackground = True
        Me.KnRatingField1.Location = New System.Drawing.Point(336, 324)
        Me.KnRatingField1.Name = "KnRatingField1"
        Me.KnRatingField1.Rating = 60
        Me.KnRatingField1.Size = New System.Drawing.Size(74, 20)
        Me.KnRatingField1.TabIndex = 2
        '
        'KnTableView1
        '
        Me.KnTableView1.AllowEmptySelection = True
        Me.KnTableView1.AllowsMultipleSelection = True
        Me.KnTableView1.AlternateBackgroundColour = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.KnTableView1.AlternatingRowBackgrounds = True
        Me.KnTableView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.KnTableView1.AutoHideScrollBars = False
        Me.KnTableView1.BackgroundColour = System.Drawing.Color.White
        Me.KnTableView1.CornerCell = Nothing
        Me.KnTableView1.DataSource = Nothing
        Me.KnTableView1.DefaultRowHeight = 20
        Me.KnTableView1.DrawHorizontalGridLines = False
        Me.KnTableView1.DrawVerticalGridLines = False
        Me.KnTableView1.GridColour = System.Drawing.Color.LightGray
        Me.KnTableView1.HeaderHeight = 20
        Me.KnTableView1.HeaderSizingMode = KNControls.KNTableView.HeaderAutoSizing.DivideEqually
        Me.KnTableView1.Location = New System.Drawing.Point(12, 27)
        Me.KnTableView1.Name = "KnTableView1"
        Me.KnTableView1.SelectedRows = CType(resources.GetObject("KnTableView1.SelectedRows"), System.Collections.ArrayList)
        Me.KnTableView1.SelectionStyling = KNControls.KNTableView.SelectionStyle.SourceListStyle
        Me.KnTableView1.Size = New System.Drawing.Size(276, 232)
        Me.KnTableView1.TabIndex = 0
        Me.KnTableView1.TableDelegate = Nothing
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip1.Size = New System.Drawing.Size(595, 24)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LickToolStripMenuItem})
        Me.FileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FileToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'LickToolStripMenuItem
        '
        Me.LickToolStripMenuItem.Name = "LickToolStripMenuItem"
        Me.LickToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.LickToolStripMenuItem.Text = "Click"
        '
        'KnProgressWheel1
        '
        Me.KnProgressWheel1.Location = New System.Drawing.Point(560, 292)
        Me.KnProgressWheel1.Name = "KnProgressWheel1"
        Me.KnProgressWheel1.Size = New System.Drawing.Size(23, 19)
        Me.KnProgressWheel1.TabIndex = 12
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(595, 358)
        Me.Controls.Add(Me.KnProgressWheel1)
        Me.Controls.Add(Me.EditableCellTable)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.KnTokenField1)
        Me.Controls.Add(Me.KnLeopardStyleHeaderButton1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.StarsField)
        Me.Controls.Add(Me.KnRatingField1)
        Me.Controls.Add(Me.KnTableView1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents KnTableView1 As KNControls.KNTableView
    Friend WithEvents KnRatingField1 As KNControls.KNRatingField
    Friend WithEvents StarsField As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents KnLeopardStyleHeaderButton1 As KNControls.KNLeopardStyleHeaderButton
    Friend WithEvents KnTokenField1 As KNControls.KNTokenField
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents EditableCellTable As KNControls.KNTableView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LickToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents KnProgressWheel1 As KNControls.KNProgressWheel

End Class
