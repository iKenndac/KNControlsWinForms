Public Class KNRatingField
    Implements KNCellContainer
    Implements KNActionCellDelegate

    Private _cell As KNRatingCell = New KNRatingCell
    Private _cellSwallowedMouse As Boolean
    Private _drawBackground As Boolean
    Private _backgroundColor As Color = Color.White

    Private _drawBorder As Boolean
    Private _borderColor As Color = Color.LightGray


    Public Event RatingChanged()


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _cell.ParentControl = Me
        _cell.CellDelegate = Me

    End Sub


    Public Function Control() As Control Implements KNCellContainer.Control
        Return Me
    End Function

    Private Sub Draw(ByVal sender As Object, ByVal e As PaintEventArgs) Handles MyBase.Paint

        Dim rect As Rectangle = New Rectangle(0, 0, MyBase.Width, MyBase.Height)

        If _drawBackground Then
            e.Graphics.FillRectangle(New SolidBrush(_backgroundColor), rect)
        End If

        If _drawBorder Then
            rect.Width -= 1
            rect.Height -= 1
            e.Graphics.DrawRectangle(New Pen(_borderColor), rect)
        End If

        _cell.DrawInFrame(e.Graphics, New Rectangle(0, 0, MyBase.Width, MyBase.Height))

    End Sub


    Private Shadows Sub MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown

        _cellSwallowedMouse = _cell.MouseDownInCell(New Point(e.X, e.Y), _
                    New Rectangle(0, 0, MyBase.Width, MyBase.Height))
        MyBase.Invalidate()

    End Sub

    Private Shadows Sub MouseMoved(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove

        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Dragging...

            If _cellSwallowedMouse = True Then

                ' Cell swallows mouse.

                If _cell.MouseDraggedInCell(New Point(e.X, e.Y), _
                            New Rectangle(0, 0, MyBase.Width, MyBase.Height)) Then
                    MyBase.Invalidate()
                End If
            End If
        End If
    End Sub

    Private Shadows Sub MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseUp

        If _cellSwallowedMouse = True Then

            ' Cell swallows mouse.


            If _cell.MouseUpInCell(New Point(e.X, e.Y), _
                        New Rectangle(0, 0, MyBase.Width, MyBase.Height)) Then
                MyBase.Invalidate()
            End If
        End If

        _cellSwallowedMouse = False



    End Sub


    Public Sub UpdateCell(ByRef cell As KNCell) Implements KNCellContainer.UpdateCell
        MyBase.Invalidate()
    End Sub

    Public Sub CellPerformedAction(ByRef Cell As KNActionCell) Implements KNActionCellDelegate.CellPerformedAction
        _cell.ObjectValue = Cell.ObjectValue
        RaiseEvent RatingChanged()
    End Sub


    Public Property Rating() As Integer
        Get
            Return CInt(_cell.ObjectValue)
        End Get
        Set(ByVal value As Integer)
            _cell.ObjectValue = value
            UpdateCell(_cell)
        End Set
    End Property

    Public Property Editable() As Boolean
        Get
            Return _cell.Editable
        End Get
        Set(ByVal value As Boolean)
            _cell.Editable = value
            UpdateCell(_cell)
        End Set
    End Property


    Public Property FillBackground() As Boolean
        Get
            Return _drawBackground
        End Get
        Set(ByVal value As Boolean)
            _drawBackground = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property BackgroundColor() As Color
        Get
            Return _backgroundColor
        End Get
        Set(ByVal value As Color)
            _backgroundColor = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property DrawBorder() As Boolean
        Get
            Return _drawBorder
        End Get
        Set(ByVal value As Boolean)
            _drawBorder = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property BorderColor() As Color
        Get
            Return _borderColor
        End Get
        Set(ByVal value As Color)
            _borderColor = value
            MyBase.Invalidate()
        End Set
    End Property

End Class
