Public Class KNCell

    Private _objValue As Object
    Private _highlighted As Boolean
    Private _state As KNCellState
    Private _parentControl As KNCellContainer
    Private _enabled As Boolean = True

    Public Enum KNCellState
        KNMixedState = -1
        KNOffState = 0
        KNOnState = 1
    End Enum

    Public Overridable Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)
        'g.FillRectangle(Brushes.Blue, frame)
    End Sub

    Public Overridable Function NewInstance() As KNCell

        Return New KNCell

    End Function

    Public Property ParentControl() As KNCellContainer
        Get
            Return _parentControl
        End Get
        Set(ByVal value As KNCellContainer)
            _parentControl = value
        End Set
    End Property



    Public Overridable Property ObjectValue() As Object
        Get
            Return _objValue
        End Get
        Set(ByVal value As Object)
            _objValue = value
        End Set
    End Property

    Public Overridable Property Highlighted() As Boolean
        Get
            Return _highlighted
        End Get
        Set(ByVal value As Boolean)
            _highlighted = value
        End Set
    End Property

    Public Overridable Property State() As KNCellState
        Get
            Return _state
        End Get
        Set(ByVal value As KNCellState)
            _state = value
        End Set
    End Property

    Public Overridable Property Enabled() As Boolean
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
            If Not ParentControl Is Nothing Then
                ParentControl.UpdateCell(Me)
            End If
        End Set
    End Property



End Class



Public Interface KNCellContainer

    Sub UpdateCell(ByRef cell As KNCell)
    Function Control() As Control

End Interface