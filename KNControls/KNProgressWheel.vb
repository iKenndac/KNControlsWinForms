Public Class KNProgressWheel
    Implements KNCellContainer

    Private _cell As KNProgressWheelCell


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _cell = New KNProgressWheelCell
        _cell.ParentControl = Me
        _cell.Enabled = Me.Enabled

    End Sub

    Public Function Control() As Control Implements KNCellContainer.Control
        Return Me
    End Function

    Public Sub Draw(ByVal sender As Object, ByVal e As PaintEventArgs) Handles MyBase.Paint

        If Not _cell Is Nothing Then
            _cell.DrawInFrame(e.Graphics, New Rectangle(0, 0, MyBase.Width, MyBase.Height))
        End If

    End Sub


    Public Sub UpdateCell(ByRef cell As KNCell) Implements KNCellContainer.UpdateCell
        MyBase.Invalidate()
    End Sub


    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal value As Boolean)
            MyBase.Enabled = value
            _cell.Enabled = value
        End Set
    End Property


End Class
