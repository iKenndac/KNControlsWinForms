<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KNTableView
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.scroller = New System.Windows.Forms.VScrollBar
        Me.SuspendLayout()
        '
        'scroller
        '
        Me.scroller.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scroller.LargeChange = 5
        Me.scroller.Location = New System.Drawing.Point(335, 21)
        Me.scroller.Maximum = 20
        Me.scroller.Name = "scroller"
        Me.scroller.Size = New System.Drawing.Size(17, 220)
        Me.scroller.TabIndex = 0
        '
        'KNTableView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.scroller)
        Me.DoubleBuffered = True
        Me.Name = "KNTableView"
        Me.Size = New System.Drawing.Size(352, 241)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents scroller As System.Windows.Forms.VScrollBar

End Class
