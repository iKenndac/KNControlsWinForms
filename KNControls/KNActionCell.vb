Public Class KNActionCell
    Inherits KNCell

    ' An action cell is a cell that you can interact with. It can contain buttons, etc, and
    ' has support for mouse tracking, etc. 

    Private _delegate As KNActionCellDelegate


    Public Overridable Function MouseDidMoveInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 

        Return False
    End Function

    Public Overridable Function MouseDownInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell claims ownership of the following drag and mouseup events.
        ' The cell is always redrawn after this function is called.

        Return False
    End Function

    Public Overridable Function MouseDraggedInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        Return False
    End Function

    Public Overridable Function MouseUpInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        Return False
    End Function

    Public Overrides Function NewInstance() As KNCell

        Return New KNActionCell

    End Function

    Public Property CellDelegate() As KNActionCellDelegate
        Get
            Return _delegate
        End Get
        Set(ByVal value As KNActionCellDelegate)
            _delegate = value
        End Set
    End Property

End Class

Public Interface KNActionCellDelegate

    Sub CellPerformedAction(ByRef Cell As KNActionCell)


End Interface