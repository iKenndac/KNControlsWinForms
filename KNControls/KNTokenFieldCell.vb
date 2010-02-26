Public Class KNTokenFieldCell
    Inherits KNActionCell

    Implements KNActionCellDelegate
    Implements KNCellContainer


    Private _contentArray As ArrayList = New ArrayList
    Private _cellArray As ArrayList = New ArrayList
    'Private _tokenCell As KNTokenCell = New KNTokenCell
    'Private _textCell As KNTextCell = New KNTextCell
    Private _currentCell As KNCell
    Private _currentCellRect As Rectangle
    Private _leftPadding As Integer = 5
    Private _cellSwallowedMouse As Boolean

    Private _editable As Boolean = True
    Private _allowsObjectChanges As Boolean = True

    Private _dragPoint As Point
    Private _dragPointIndex As Integer
    Private _drawDragPoint As Boolean

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)


        Dim bounds As Rectangle = New Rectangle(frame.X, frame.Y, frame.Width - 1, frame.Height - 1)

        Dim xProgress As Integer = _leftPadding 'Start at 10px in for padding

        _allowsObjectChanges = False
        For Each item As Object In _contentArray

            If _drawDragPoint AndAlso _contentArray.IndexOf(item) = _dragPointIndex Then
                g.DrawLine(Pens.LightBlue, frame.X + xProgress, bounds.Y + 3, frame.X + xProgress, frame.Y + bounds.Height - 4)
            End If

            Dim cellToDraw As KNCell = Nothing
            Dim cellWidth As Integer

            If TypeOf item Is TokenizableObject Then
                Dim token As TokenizableObject = CType(item, TokenizableObject)

                Dim _tokenCell As KNTokenCell = CellForObject(item)
                _tokenCell.ObjectValue = token
                _tokenCell.ParentControl = Me

                cellToDraw = _tokenCell

                cellWidth = _tokenCell.PreferredWidth(g)

            ElseIf TypeOf item Is String Then

                Dim str As String = CType(item, String)

                Dim _textCell As KNTextCell = CellForObject(item)
                _textCell.ObjectValue = str
                _textCell.ParentControl = Me
                cellToDraw = _textCell
                cellWidth = _textCell.PreferredWidth(g)

            End If

            Dim cellRect As Rectangle = New Rectangle(bounds.X + xProgress, bounds.Y + 1, cellWidth, bounds.Height - 1)

            cellToDraw.Highlighted = True
            cellToDraw.DrawInFrame(g, cellRect)

            xProgress += cellWidth


        Next
        _allowsObjectChanges = True

        If _drawDragPoint AndAlso _contentArray.Count = _dragPointIndex Then
            g.DrawLine(Pens.LightBlue, bounds.X + xProgress, bounds.Y + 3, bounds.X + xProgress, bounds.Y + bounds.Height - 4)
        End If


    End Sub



    Public Sub CellPerformedAction(ByRef Cell As KNActionCell) Implements KNActionCellDelegate.CellPerformedAction
        Dim index As Integer = _cellArray.IndexOf(Cell)

        If index > -1 Then

            If TypeOf _contentArray.Item(index) Is String And _allowsObjectChanges Then

                If Cell.ObjectValue = "" Then
                    _contentArray.RemoveAt(index)
                    _cellArray.RemoveAt(index)
                Else
                    _contentArray.Item(index) = Cell.ObjectValue
                End If

            End If


        End If

        CellDelegate.CellPerformedAction(Cell)

    End Sub

    Public Overrides Function NewInstance() As KNCell
        Dim cell As New KNTokenFieldCell
        cell.Editable = Me.Editable
        Return cell
    End Function

    Public Function Control() As System.Windows.Forms.Control Implements KNCellContainer.Control
        Return ParentControl.Control()
    End Function

    Public Sub UpdateCell(ByRef cell As KNCell) Implements KNCellContainer.UpdateCell
        Dim index As Integer = _cellArray.IndexOf(cell)

        If index > -1 Then

            If TypeOf _contentArray.Item(index) Is String Then


                _contentArray.Item(index) = cell.ObjectValue


            End If

            CellDelegate.CellPerformedAction(cell)
        End If

        ParentControl.Control.Invalidate()
    End Sub

    Private Function CellForObject(ByVal obj As Object) As KNCell
        ' Assume cellArray is the correct size

        Dim index As Integer = _contentArray.IndexOf(obj)

        If index > -1 Then

            Dim cell As KNCell = _cellArray.Item(index)

            If cell Is Nothing Then
                If TypeOf obj Is String Then
                    cell = New KNTextCell
                    CType(cell, KNTextCell).IsEditable = True
                    CType(cell, KNTextCell).TextHighlightColor = Color.Black
                    cell.Highlighted = True
                ElseIf TypeOf obj Is KNTokenCell Then
                    cell = New KNTokenCell
                End If
                _cellArray.Item(index) = cell
                cell.ParentControl = Me
                Return cell
            End If

            If TypeOf obj Is String And Not TypeOf cell Is KNTextCell Then
                cell = New KNTextCell
                cell.ParentControl = Me
                CType(cell, KNTextCell).IsEditable = True
                CType(cell, KNTextCell).TextHighlightColor = Color.Black
                cell.Highlighted = True
                _cellArray.Item(index) = cell
                Return cell
            End If

            If TypeOf obj Is TokenizableObject And Not TypeOf cell Is KNTokenCell Then
                cell = New KNTokenCell
                _cellArray.Item(index) = cell
                Return cell
            End If

            Return cell

        End If

        Return Nothing


    End Function

    Private Sub RebuildCellList()

        _cellArray = New ArrayList

        For Each obj As Object In _contentArray

            Dim cell As KNActionCell = Nothing

            If TypeOf obj Is String Then
                cell = New KNTextCell
                CType(cell, KNTextCell).IsEditable = True
                CType(cell, KNTextCell).TextHighlightColor = Color.Black
                cell.Highlighted = True

            ElseIf TypeOf obj Is TokenizableObject Then
                cell = New KNTokenCell
            End If
            cell.ParentControl = Me
            cell.CellDelegate = Me
            _cellArray.Add(cell)
        Next

    End Sub

    Public Overrides Property ObjectValue() As Object
        Get
            Return Items
        End Get
        Set(ByVal value As Object)
            Items = value
        End Set
    End Property


    Public Property Items() As Object()
        Get
            Return _contentArray.ToArray
        End Get
        Set(ByVal value As Object())
            If _allowsObjectChanges Then
                _contentArray = New ArrayList(value)
                RebuildCellList()
            Else
                Throw New Exception("Can't change objects when drawing!")
            End If

            'ParentControl.Control.Invalidate()
        End Set
    End Property


    Public Property Editable() As Boolean
        Get
            Return _editable
        End Get
        Set(ByVal value As Boolean)
            _editable = value
        End Set
    End Property


    Public Function BestIndexForInsertingObjectAtLocation(ByVal location As Point) As Integer

        '                   _________
        ' Given the token: (_________)
        ' The cutoff point is:  ^     (halfway)

        Dim currentCellWidth As Integer = 0
        Dim cellEndX As Integer = _leftPadding

        Dim currentIndex As Integer = 0
        Dim count As Integer = _contentArray.Count - 1

        For currentIndex = 0 To count

            Dim item As Object = _contentArray.Item(currentIndex)

            If TypeOf item Is TokenizableObject Then
                Dim _tokenCell As KNTokenCell = CellForObject(item)
                _tokenCell.ObjectValue = item
                currentCellWidth = _tokenCell.PreferredWidth

            ElseIf TypeOf item Is String Then
                Dim _textCell As KNTextCell = CellForObject(item)
                _textCell.ObjectValue = item
                currentCellWidth = _textCell.PreferredWidth

            End If

            If location.X < cellEndX + (currentCellWidth / 2) Then
                ' Before the current item
                Return currentIndex
            End If

            cellEndX += currentCellWidth


        Next

        ' If we get here, then the drag is past the end of the objects!

        Return _contentArray.Count

    End Function

#Region "Mouse"

    Public Overrides Function MouseDidMoveInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 

        ' First, if there's a current cell and the mouse isn't in it any more,
        ' Send it a mouseMoved event with the point outside the frame 

        If Not _currentCell Is Nothing AndAlso _cellSwallowedMouse Then
            Return False

        End If

        If Not Editable Then
            Return False
        End If

        If Not _currentCell Is Nothing Then
            If TypeOf (_currentCell) Is KNActionCell Then
                If Not _currentCellRect.Contains(relativePoint.X, relativePoint.Y) Then
                    CType(_currentCell, KNActionCell).MouseDidMoveInCell(New Point(relativePoint.X - _currentCellRect.X, relativePoint.Y - _currentCellRect.Y), New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height))
                    ParentControl.UpdateCell(Me) '.Invalidate(_currentCellRect)

                End If
            End If
        End If



        Dim cellEndX As Integer = _leftPadding
        Dim currentCellWidth As Integer = 0
        Dim _hoveredCell As KNCell = Nothing
        Dim currentCell As KNCell = Nothing

        For Each item As Object In _contentArray
            If TypeOf item Is TokenizableObject Then
                Dim _tokenCell As KNTokenCell = CellForObject(item)
                _tokenCell.ObjectValue = item
                currentCellWidth = _tokenCell.PreferredWidth
                cellEndX += currentCellWidth
                currentCell = _tokenCell



            ElseIf TypeOf item Is String Then
                Dim _textCell As KNTextCell = CellForObject(item)
                _textCell.ObjectValue = item
                currentCellWidth = _textCell.PreferredWidth
                cellEndX += currentCellWidth
                currentCell = _textCell


            End If

            If relativePoint.X <= cellEndX - 2 And relativePoint.X >= (cellEndX - currentCellWidth) + 2 Then
                _hoveredCell = currentCell

                If TypeOf _hoveredCell Is KNTextCell Then
                    ParentControl.Control.Cursor = Cursors.IBeam
                Else
                    ParentControl.Control.Cursor = Cursors.Arrow
                End If

                Exit For
            End If
        Next

        If Not _hoveredCell Is Nothing AndAlso TypeOf _hoveredCell Is KNActionCell Then

            'We're over an action cell!
            Dim cellRect As Rectangle = New Rectangle(cellEndX - currentCellWidth, 2, _
                                                currentCellWidth, relativeFrame.Height - 3)
            Dim shouldDraw As Boolean

            _currentCellRect = cellRect
            _currentCell = _hoveredCell

            shouldDraw = CType(_hoveredCell, KNActionCell).MouseDidMoveInCell(New Point(relativePoint.X - cellRect.X, relativePoint.Y - cellRect.Y), New Rectangle(0, 0, cellRect.Width, cellRect.Height))

            If shouldDraw Then
                Return True
            End If
        Else
            _currentCell = Nothing

            ParentControl.Control.Cursor = Cursors.IBeam
        End If

        Return False
    End Function

    Public Overrides Function MouseDownInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell claims ownership of the following drag and mouseup events.
        ' The cell is always redrawn after this function is called.

        If Not Editable Then
            Return False
        End If

        If Not _currentCell Is Nothing Then
            If TypeOf (_currentCell) Is KNActionCell Then
                If _currentCellRect.Contains(relativePoint) Then
                    _cellSwallowedMouse = CType(_currentCell, KNActionCell).MouseDownInCell(New Point(relativePoint.X - _currentCellRect.X, relativePoint.Y - _currentCellRect.Y), _
                                New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height))
                    Return True
                End If
            End If
        End If

        ' Not directly over a cell...

        Dim index As Integer = BestIndexForInsertingObjectAtLocation(relativePoint)

        ' Check if an immideately adjacent item is a string and edit that rather
        ' than creating a new one

        If index < _contentArray.Count AndAlso TypeOf _contentArray.Item(index) Is String Then

            CType(_cellArray.Item(index), KNTextCell).StartEditing()

        ElseIf index > 0 AndAlso TypeOf _contentArray.Item(index - 1) Is String Then

            CType(_cellArray.Item(index - 1), KNTextCell).StartEditing()

        Else

            _contentArray.Insert(index, "")
            Dim cell As New KNTextCell
            CType(cell, KNTextCell).IsEditable = True
            cell.CellDelegate = Me
            cell.ParentControl = Me
            cell.TextHighlightColor = Color.Black
            cell.Highlighted = True
            _cellArray.Insert(index, cell)

            ParentControl.Control.Invalidate()

            cell.StartEditing()

            CellDelegate.CellPerformedAction(Me)
        End If

        Return True
    End Function

    Public Overrides Function MouseDraggedInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        If Not _currentCell Is Nothing And _cellSwallowedMouse = True Then

            ' Cell swallows mouse.

            If TypeOf (_currentCell) Is KNActionCell Then
                If CType(_currentCell, KNActionCell).MouseDraggedInCell(New Point(relativePoint.X - _currentCellRect.X, relativePoint.Y - _currentCellRect.Y), _
                            New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height)) Then
                    Return True
                End If
            End If


        End If

        Return False
    End Function

    Public Overrides Function MouseUpInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        If Not _currentCell Is Nothing And _cellSwallowedMouse = True Then

            ' Cell swallows mouse.

            _cellSwallowedMouse = False

            If TypeOf (_currentCell) Is KNActionCell Then
                If CType(_currentCell, KNActionCell).MouseUpInCell(New Point(relativePoint.X - _currentCellRect.X, relativePoint.Y - _currentCellRect.Y), _
                            New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height)) Then
                    Return True
                End If
            End If

        End If

        Return False
    End Function

#End Region

#Region "Drag and Drop"


    Public Sub DrEnter(ByVal sender As Object, ByVal e As DragEventArgs)

        If e.Data.GetDataPresent(DataFormats.Serializable, False) Then
            If e.AllowedEffect = DragDropEffects.Copy Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.Move
            End If
        Else
            e.Effect = DragDropEffects.None
        End If

        _drawDragPoint = True
        _dragPoint = Me.Control.PointToClient(New Point(e.X, e.Y))
        _dragPointIndex = BestIndexForInsertingObjectAtLocation(_dragPoint)
        Me.Control.Invalidate()


    End Sub

    Public Sub DrOver(ByVal Sender As Object, ByVal e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.Serializable, False) Then
            If e.AllowedEffect = DragDropEffects.Copy Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.Move
            End If

        Else
            e.Effect = DragDropEffects.None
        End If

        _drawDragPoint = True
        _dragPoint = Me.Control.PointToClient(New Point(e.X, e.Y))
        _dragPointIndex = BestIndexForInsertingObjectAtLocation(_dragPoint)
        Me.Control.Invalidate()


    End Sub

    Public Sub DrExit(ByVal sender As Object, ByVal e As EventArgs)

        _drawDragPoint = False
        '_dragPoint = Me.PointToClient(New Point(e.X, e.Y))
        Me.Control.Invalidate()



    End Sub

    Public Sub DrDrop(ByRef sender As Object, ByVal e As DragEventArgs)

        Dim token As TokenizableObject = (e.Data.GetData(DataFormats.Serializable))


        'ReDim Preserve _contentArray(UBound(_contentArray) + 1)
        '_contentArray(UBound(_contentArray)) = token
        'MyBase.Invalidate()


        Dim localPoint = Me.Control.PointToClient(New Point(e.X, e.Y))
        Dim index As Integer = Me.BestIndexForInsertingObjectAtLocation(localPoint)

        Dim oldIndex As Integer

        If e.AllowedEffect = DragDropEffects.Move Then
            oldIndex = _contentArray.IndexOf(token)

            If index >= oldIndex Then
                'index = index - 1
            Else
                oldIndex += 1
            End If
        End If

        _contentArray.Insert(index, token.Clone)
        Dim cell As KNTokenCell = New KNTokenCell
        cell.CellDelegate = Me
        _cellArray.Insert(index, cell)

        _drawDragPoint = False

        If e.AllowedEffect = DragDropEffects.Move Then
            _contentArray.RemoveAt(oldIndex)
            _cellArray.RemoveAt(oldIndex)
        End If

        Me.CellDelegate.CellPerformedAction(Me)
        '5Me.Control.Invalidate()




    End Sub

#End Region

    Private KNNotificationDelegate


    Public Sub BackspaceHappenedAtBeginningOfTextField(ByRef notification As KNNotification)

        Dim cell As KNCell

        If TypeOf notification.Sender Is KNCell Then
            cell = notification.Sender
        Else
            Return
        End If

        Dim cellIndex As Integer = _cellArray.IndexOf(cell)

        If cellIndex > 0 Then

            If _allowsObjectChanges Then

                If Not TypeOf (_contentArray.Item(cellIndex - 1)) Is String Then
                    _contentArray.RemoveAt(cellIndex - 1)
                    _cellArray.RemoveAt(cellIndex - 1)
                    cellIndex -= 1
                End If

            End If



            If cellIndex > 1 Then

                If TypeOf (_contentArray.Item(cellIndex - 1)) Is String Then
                    ' We can match!!!!!!11!
                    ' Our item is now at CellIndex - 1

                    Dim preText As String = _contentArray.Item(cellIndex - 1)
                    Dim location As Integer = preText.Length

                    _contentArray.Item(cellIndex - 1) = preText + cell.ObjectValue

                    CType(cell, KNTextCell).EndTextEditing()

                    If _contentArray.Count > cellIndex Then
                        _contentArray.RemoveAt(cellIndex)
                        _cellArray.RemoveAt(cellIndex)
                    End If

                    Dim newEditor As KNTextCell = _cellArray.Item(cellIndex - 1)
                    newEditor.ObjectValue = _contentArray.Item(cellIndex - 1)
                    KNEditableCell.SharedTextEditor.Text = newEditor.ObjectValue
                    'newEditor.StartEditing()

                    cell = newEditor

                End If


            End If

        End If

        CellDelegate.CellPerformedAction(cell)

    End Sub

    Public Sub New()

        With KNNotificationCentre.DefaultNotificationCentre
            .AddObserverForNotificationName(New KNNotificationDelegate(AddressOf BackspaceHappenedAtBeginningOfTextField), _
                                 "KNTextCell_BackspaceWasPressedAtBeginningOfField")
        End With

    End Sub

    Protected Overrides Sub Finalize()

        KNNotificationCentre.DefaultNotificationCentre.RemoveObserver(Me)

        MyBase.Finalize()
    End Sub
End Class
