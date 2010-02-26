Public Class KNTokenField
    Implements KNCellContainer
    Implements KNActionCellDelegate


    Private cell As KNTokenFieldCell
    Private _cellSwallowedMouse As Boolean

    Public Event ItemsDidChange()


    Public Sub New()

        cell = New KNTokenFieldCell
        cell.ParentControl = Me
        cell.CellDelegate = Me

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.AllowDrop = True
        ' _textCell.CellDelegate = Me
        '_tokenCell.CellDelegate = Me

    End Sub

    Private Sub Draw(ByVal sender As Object, ByVal e As PaintEventArgs) Handles MyBase.Paint

        If Not TextBoxRenderer.IsSupported Then
            e.Graphics.FillRectangle(Brushes.White, Bounds)
            e.Graphics.DrawRectangle(Pens.DarkGray, Bounds)
        Else
            TextBoxRenderer.DrawTextBox(e.Graphics, New Rectangle(0, 0, MyBase.Width, MyBase.Height), VisualStyles.TextBoxState.Normal)
        End If

        cell.DrawInFrame(e.Graphics, New Rectangle(0, 0, MyBase.Width, MyBase.Height))


    End Sub


    Private Sub mouseMoved(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove

        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Drag

            If _cellSwallowedMouse Then
                If cell.MouseDraggedInCell(e.Location, New Rectangle(0, 0, MyBase.Width, MyBase.Height)) Then
                    MyBase.Invalidate()
                End If
            End If

        Else
            If cell.MouseDidMoveInCell(e.Location, New Rectangle(0, 0, MyBase.Width, MyBase.Height)) Then
                MyBase.Invalidate()
            End If

        End If


    End Sub


    Private Sub mDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown

        _cellSwallowedMouse = cell.MouseDownInCell(e.Location, New Rectangle(0, 0, MyBase.Width, MyBase.Height))

    End Sub

    Private Sub mUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseUp

        If _cellSwallowedMouse Then
            If cell.MouseUpInCell(e.Location, New Rectangle(0, 0, MyBase.Width, MyBase.Height)) Then
                MyBase.Invalidate()
            End If
        End If
        _cellSwallowedMouse = False

    End Sub


#Region "Mouse"

    Private Sub DrEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles MyBase.DragEnter
        If Me.Editable Then
            cell.DrEnter(sender, e)
        End If
    End Sub

    Private Sub DrOver(ByVal Sender As Object, ByVal e As DragEventArgs) Handles MyBase.DragOver
        If Me.Editable Then
            cell.DrOver(Sender, e)
        End If

    End Sub

    Private Sub DrExit(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.DragLeave
        If Me.Editable Then
            cell.DrExit(sender, e)
        End If

    End Sub

    Private Sub DrDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles MyBase.DragDrop
        If Me.Editable Then
            cell.DrDrop(sender, e)
            RaiseEvent ItemsDidChange()

        End If
    End Sub




#End Region

    Private Sub Resized(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        MyBase.Invalidate()
    End Sub


    Public Property Items() As Object()
        Get
            Return cell.Items
        End Get
        Set(ByVal value As Object())
            cell.Items = value
            MyBase.Invalidate()
        End Set
    End Property


    Public Property Editable() As Boolean
        Get
            Return cell.Editable
        End Get
        Set(ByVal value As Boolean)

            cell.Editable = value
        End Set
    End Property


#Region "CellContainer Delegates"

    Public Function Control() As System.Windows.Forms.Control Implements KNCellContainer.Control
        Return Me
    End Function

    Public Sub UpdateCell(ByRef cell As KNCell) Implements KNCellContainer.UpdateCell
        MyBase.Invalidate()
    End Sub

#End Region


    Public Sub CellPerformedAction(ByRef Cell As KNActionCell) Implements KNActionCellDelegate.CellPerformedAction
        RaiseEvent ItemsDidChange()
        MyBase.Invalidate()
    End Sub

End Class
