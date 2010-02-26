Imports System.Drawing.Drawing2D

Public Class KNTableView
    Implements KNTableColumnDelegate
    Implements KNCellContainer

    ' Instance variables 

    Private _columns(-1) As KNTableColumn
    Private _cornerCell As KNCell
    Private _dataSource As KNTableViewDataSource
    Private _delegate As KNTableViewDelegate
    'Private _selectedRow As Integer = -1
    Private _selectedRows As ArrayList = New ArrayList()
    Private _sortOrder As SortOrder = SortOrder.Ascending

    Private _currentCellRect As Rectangle ' The frame of the cell that the mouse is currently focused on
    Private _currentCell As KNCell ' The cell the mouse is currently focused on (over, dragging in, etc)
    Private _cellSwallowedMouse As Boolean

    ' Properties
    Private _defaultRowHeight As Integer = 20
    Private _headerHeight As Integer = 20
    Private _backgroundColor As Color = Color.White
    Private _drawHorizontalGridLines As Boolean = False
    Private _drawVerticalGridLines As Boolean = False
    Private _gridColor As Color = Color.LightGray
    Private _alternateRowColor As Color = Color.FromArgb(237, 243, 254)
    Private _alternatingRow As Boolean = False
    Private _headerSizing As HeaderAutoSizing = HeaderAutoSizing.NoAutoSizing
    Private _selectionStyle As SelectionStyle = SelectionStyle.FlatStyle
    Private _autoHideScrollBars As Boolean = False
    Private _allowsMultipleSelection As Boolean = False
    Private _allowEmptySelection As Boolean = True

    ' Click Handling

    Private _isWaitingForSecondClick As Boolean
    Private _doubleClickHasHappened As Boolean
    Private _mouseDownPoint As Point
    Private _firstClickPoint As Point
    Private WithEvents _doubleClickTimer As Timer
    Private _clickedRow As Integer
    Private _clickedColumn As KNTableColumn


    ' Enums

    Public Enum HeaderAutoSizing

        NoAutoSizing = 0
        DivideEqually = 1
        ResizeLast = 2

    End Enum

    Public Enum SelectionStyle
        FlatStyle = 0
        SourceListStyle = 1
    End Enum

    ' Events 

    ' Public Event RowClicked(ByVal Row As Integer, ByRef Column As KNTableColumn)
    Public Event SelectionChanged(ByVal Rows As ArrayList)
    Public Event RowDoubleClicked(ByVal Row As Integer, ByRef Column As KNTableColumn)
    Public Event CellPerformedAction(ByVal Row As Integer, ByRef Column As KNTableColumn, ByVal Cell As KNActionCell)



    Private Sub KNTableView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        AddHandler scroller.MouseDown, New MouseEventHandler(AddressOf scrollerMouseDown)

    End Sub

    Public Function Control() As Control Implements KNCellContainer.Control
        Return Me
    End Function


    Private Sub DidReceiveFocus(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.GotFocus
        MyBase.Invalidate()
    End Sub

    Private Sub DidLoseFocus(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.LostFocus
        MyBase.Invalidate()
    End Sub

    


    Private Sub Draw(ByVal sender As Object, ByVal e As Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Dim g As Graphics = e.Graphics

        Dim headersRect As Rectangle = New Rectangle(0, 0, MyBase.Width - 1, _headerHeight)
        Dim rowsRect As Rectangle = New Rectangle(0, _headerHeight + 1, MyBase.Width - scroller.Width, MyBase.Height - _headerHeight - 2)

        'g.DrawRectangle(Pens.Black, headerRect)

        'g.DrawRectangle(Pens.Blue, rowsRect)

        g.FillRectangle(New SolidBrush(_backgroundColor), New Rectangle(0, 0, MyBase.Width, MyBase.Height))

        ' Alllllll righty then!

        ' Rows aren't actually drawn here :-(
        ' Well, actually, they are - the background is drawn, but then the content 
        ' drawing is passed off to each cell. 

        Dim topRow As Integer = scroller.Value
        Dim visibleRowCount As Integer = Math.Floor(rowsRect.Height / _defaultRowHeight)
        Dim actualRowCount As Integer

        If Not _dataSource Is Nothing Then
            actualRowCount = _dataSource.NumberOfItemsInTableView(Me)
            If actualRowCount > 0 Then
                scroller.Maximum = actualRowCount - 1
                scroller.LargeChange = visibleRowCount
            End If
            If (visibleRowCount >= actualRowCount) Then
                scroller.Enabled = False
                If (_autoHideScrollBars) Then
                    scroller.Width = 0
                    scroller.Visible = False
                End If
            Else
                scroller.Enabled = True
                If (_autoHideScrollBars) Then
                    scroller.Width = 17
                    scroller.Visible = True
                End If
            End If
        End If

        If topRow = -1 Then
            topRow = 0
        End If

        Dim currentRow As Integer = 0
        Dim rowRect As Rectangle
        Dim columnRect As Rectangle
        Dim columnStartX As Integer

        For currentRow = topRow To topRow + visibleRowCount

            columnStartX = 0

            rowRect = New Rectangle(0, rowsRect.Y + ((currentRow - topRow) * _defaultRowHeight), rowsRect.Width, _defaultRowHeight)

            'Draw background 

            If (_selectedRows.Contains(currentRow)) Then

                Select Case _selectionStyle
                    Case SelectionStyle.FlatStyle

                        If Me.ContainsFocus Then
                            g.FillRectangle(New SolidBrush(SystemColors.Highlight), rowRect)
                        Else
                            g.FillRectangle(New SolidBrush(SystemColors.ControlDark), rowRect)
                        End If



                    Case SelectionStyle.SourceListStyle

                        Dim startColor As Color
                        Dim endColor As Color

                        If Me.ContainsFocus Then
                            startColor = Color.FromArgb(15, 94, 217)
                            endColor = Color.FromArgb(77, 153, 235)
                        Else
                            startColor = Color.FromArgb(107, 107, 107)
                            endColor = Color.FromArgb(152, 152, 152)
                        End If

                        Dim gradient As LinearGradientBrush = New LinearGradientBrush(rowRect, endColor, startColor, LinearGradientMode.Vertical)

                        g.FillRectangle(gradient, rowRect)
                        g.DrawLine(New Pen(startColor), rowRect.X, rowRect.Y, rowRect.X + rowRect.Width, rowRect.Y)

                End Select


            ElseIf (_alternatingRow = True) Then
                If currentRow Mod 2 = 0 Then
                    g.FillRectangle(New SolidBrush(_alternateRowColor), rowRect)
                End If
            End If



            ' Now draw row cells

            If (currentRow < actualRowCount) Then
                For Each col As KNTableColumn In _columns

                    columnRect = New Rectangle(columnStartX, rowRect.Y, col.Width, rowRect.Height)
                    columnStartX += col.Width

                    If Not _dataSource Is Nothing Then
                        col.CellForRow(currentRow).Highlighted = _selectedRows.Contains(currentRow)
                        col.CellForRow(currentRow).ObjectValue = _dataSource.ObjectForRow(Me, col, currentRow)

                    End If

                    col.CellForRow(currentRow).ParentControl = Me
                    col.CellForRow(currentRow).DrawInFrame(g, columnRect)


                Next

            End If

            If (_drawHorizontalGridLines = True) Then
                g.DrawLine(New Pen(_gridColor), 0, rowRect.Y + rowRect.Height, rowRect.Width, rowRect.Y + rowRect.Height)
            End If

            ' g.DrawRectangle(Pens.Black, rowRect)

        Next

        ' Go through columns again, drawing lines and headers

        columnStartX = 0

        For Each col As KNTableColumn In _columns

            Dim headerRect As Rectangle = New Rectangle(columnStartX, headersRect.Y, col.Width, headersRect.Height)

            CType(col.HeaderCell, KNHeaderCell).DrawInFrame(g, headerRect, _sortOrder)
            'g.DrawString(headerRect.Width.ToString, New Font("Arial", 10), Brushes.Black, headerRect.X + 2, 2)

            columnStartX += col.Width

            If (_drawVerticalGridLines = True) Then
                g.DrawLine(New Pen(_gridColor), columnStartX - 1, rowsRect.Y, columnStartX - 1, rowsRect.Y + rowsRect.Height)
            End If
        Next

        If (headersRect.Width - columnStartX) > 0 Then
            If Not _cornerCell Is Nothing Then
                _cornerCell.DrawInFrame(g, New Rectangle(columnStartX, headersRect.Y, headersRect.Width - columnStartX + 1, headersRect.Height))
            End If
        End If


    End Sub

    ' Mouse

    Private Sub MouesWheel(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseWheel

        Dim newVal As Integer = scroller.Value - (e.Delta * SystemInformation.MouseWheelScrollLines / 120)

        If (newVal < scroller.Minimum) Then
            scroller.Value = scroller.Minimum
        ElseIf newVal > scroller.Maximum - scroller.LargeChange Then
            'scroller.Maximum - scroller.LargeChange >= 0
            Dim val As Integer
            val = 1 + scroller.Maximum - scroller.LargeChange
            If (val < 0) Then
                val = 0
            End If
            scroller.Value = val
        Else
            If newVal >= 0 Then
                scroller.Value = newVal
            End If
        End If



    End Sub


    Private Enum KNHeaderDragAction
        NoAction = 0
        ResizeAction = 1
    End Enum

    Dim _actionedColumn As KNTableColumn
    Dim _dragAction As KNHeaderDragAction
    Dim _columnStartX As Integer

    Private Shadows Sub MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown

        _actionedColumn = Nothing
        _dragAction = KNHeaderDragAction.NoAction
        Me.Focus()
        Me.Select()

        If e.Y > _headerHeight And e.X < MyBase.Width - scroller.Width Then

            ' On a row! 

            If Not _dataSource Is Nothing Then

                If Not _currentCell Is Nothing Then
                    If TypeOf (_currentCell) Is KNActionCell Then
                        If _currentCellRect.Contains(e.Location) Then
                            _cellSwallowedMouse = CType(_currentCell, KNActionCell).MouseDownInCell(New Point(e.X - _currentCellRect.X, e.Y - _currentCellRect.Y), _
                                        New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height))
                            MyBase.Invalidate(_currentCellRect)
                        End If
                    End If
                End If

                Dim row As Integer = scroller.Value + Math.Floor(((e.Y - _headerHeight) / _defaultRowHeight))

                If row < _dataSource.NumberOfItemsInTableView(Me) And row > -1 Then

                    If _selectedRows.Contains(row) Then
                        If My.Computer.Keyboard.CtrlKeyDown Then
                            _selectedRows.Remove(row)
                        End If
                    Else

                        If My.Computer.Keyboard.CtrlKeyDown And _allowsMultipleSelection Then


                            If Not _delegate Is Nothing Then
                                If _delegate.ShouldSelectRow(Me, row) Then
                                    _selectedRows.Add(row)
                                End If
                            Else
                                _selectedRows.Add(row)
                            End If

                        ElseIf My.Computer.Keyboard.ShiftKeyDown And _allowsMultipleSelection Then

                            Dim minRow As Integer = Integer.MaxValue
                            Dim maxRow As Integer = Integer.MinValue

                            For Each index As Integer In _selectedRows
                                If index < minRow Then
                                    minRow = index
                                End If

                                If index > maxRow Then
                                    maxRow = index
                                End If
                            Next

                            If row > maxRow Then
                                maxRow = row
                            End If

                            If row < minRow Then
                                minRow = row
                            End If

                            _selectedRows.Clear()

                            Dim currentRow As Integer = minRow

                            For currentRow = minRow To maxRow
                                If Not _delegate Is Nothing Then
                                    If _delegate.ShouldSelectRow(Me, currentRow) Then
                                        _selectedRows.Add(currentRow)
                                    End If
                                Else
                                    _selectedRows.Add(currentRow)
                                End If
                            Next

                        Else

                            If Not _delegate Is Nothing Then
                                If _delegate.ShouldSelectRow(Me, row) Then
                                    _selectedRows.Clear()
                                    _selectedRows.Add(row)
                                End If
                            Else
                                _selectedRows.Clear()
                                _selectedRows.Add(row)
                            End If
                        End If

                    End If


                Else
                    If _allowEmptySelection Then
                        _selectedRows.Clear()
                    End If
                End If


                RaiseEvent SelectionChanged(_selectedRows)


                _mouseDownPoint = e.Location
                _clickedColumn = ColumnForX(e.X)
                _clickedRow = row

                If Not _cellSwallowedMouse Then
                    ' Set up for a click? 
                End If

                MyBase.Invalidate()

            End If

        ElseIf e.Y <= _headerHeight Then

            ' This could either be a header click or the start of a header resize
            ' The 'buffer' for a resize is 3px either side of the edge of the column

            Dim colEndX As Integer = 0

            For Each col As KNTableColumn In _columns
                colEndX += col.Width

                If e.X <= colEndX + 3 And e.X >= colEndX - col.Width Then
                    _actionedColumn = col
                    Exit For

                End If
            Next

            If Not _actionedColumn Is Nothing Then

                _columnStartX = colEndX - _actionedColumn.Width

                If e.X >= colEndX - 3 And _actionedColumn.UserResizable Then
                    ' Resize! 
                    _dragAction = KNHeaderDragAction.ResizeAction

                Else
                    ' Click

                    _actionedColumn.HeaderCell.State = KNCell.KNCellState.KNOnState
                    MyBase.Invalidate(New Rectangle(colEndX - _actionedColumn.Width, 0, _actionedColumn.Width, _headerHeight))
                End If

            End If


        End If
    End Sub

    Private Shadows Sub MouseMoved(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Dragging...

            If Not _currentCell Is Nothing And _cellSwallowedMouse = True Then

                ' Cell swallows mouse.

                If TypeOf (_currentCell) Is KNActionCell Then
                    If CType(_currentCell, KNActionCell).MouseDraggedInCell(New Point(e.X - _currentCellRect.X, e.Y - _currentCellRect.Y), _
                                New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height)) Then
                        MyBase.Invalidate(_currentCellRect)
                    End If
                End If

                Return
            End If

            If _dragAction = KNHeaderDragAction.ResizeAction Then

                If Not _actionedColumn Is Nothing Then
                    Dim newWidth As Integer = e.X - _columnStartX

                    If newWidth < _actionedColumn.MinimumSize Then
                        _actionedColumn.Width = _actionedColumn.MinimumSize
                    ElseIf newWidth > _actionedColumn.MaximumSize Then
                        _actionedColumn.Width = _actionedColumn.MaximumSize
                    Else
                        _actionedColumn.Width = newWidth
                    End If

                    MyBase.Invalidate()

                End If

            ElseIf _dragAction = KNHeaderDragAction.NoAction Then

                If Not _actionedColumn Is Nothing Then
                    If e.X >= _columnStartX And e.X <= _columnStartX + _actionedColumn.Width And e.Y >= 0 And e.Y <= _headerHeight Then
                        ' Over the actioned column
                        If _actionedColumn.HeaderCell.State <> KNCell.KNCellState.KNOnState Then
                            _actionedColumn.HeaderCell.State = KNCell.KNCellState.KNOnState
                            MyBase.Invalidate(New Rectangle(_columnStartX, 0, _actionedColumn.Width, _headerHeight))
                        End If

                    Else
                        ' not! 
                        If _actionedColumn.HeaderCell.State <> KNCell.KNCellState.KNOffState Then
                            _actionedColumn.HeaderCell.State = KNCell.KNCellState.KNOffState
                            MyBase.Invalidate(New Rectangle(_columnStartX, 0, _actionedColumn.Width, _headerHeight))
                        End If

                    End If
                End If
            End If

        ElseIf e.Button = Windows.Forms.MouseButtons.None Then
            ' Just movin' around...

            If e.Y <= _headerHeight Then
                ' Over the headers.

                Dim colEndX As Integer = 0

                For Each col As KNTableColumn In _columns
                    colEndX += col.Width

                    If e.X <= colEndX + 3 And e.X >= colEndX - 3 Then
                        If col.UserResizable = True Then
                            MyBase.Cursor = Cursors.VSplit
                        End If
                        Return
                    End If
                Next
                MyBase.Cursor = Cursors.Arrow
            Else
                ' Over the rows 

                MyBase.Cursor = Cursors.Arrow

                If e.X < MyBase.Width - scroller.Width Then

                    ' On a row! 

                    ' First, if there's a current cell and the mouse isn't in it any more,
                    ' Send it a mouseMoved event with the point outside the frame 

                    If Not _currentCell Is Nothing Then
                        If TypeOf (_currentCell) Is KNActionCell Then
                            If Not _currentCellRect.Contains(e.X, e.Y) Then
                                If CType(_currentCell, KNActionCell).MouseDidMoveInCell(New Point(e.X - _currentCellRect.X, e.Y - _currentCellRect.Y), New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height)) Then
                                    MyBase.Invalidate(_currentCellRect)
                                End If
                            End If
                        End If
                    End If

                    If Not _dataSource Is Nothing Then

                        Dim row As Integer = scroller.Value + Math.Floor(((e.Y - _headerHeight) / _defaultRowHeight))

                        If row >= _dataSource.NumberOfItemsInTableView(Me) Then
                            row = -1
                        End If

                        Dim colEndX As Integer = 0
                        Dim _hoveredCol As KNTableColumn = Nothing

                        For Each col As KNTableColumn In _columns
                            colEndX += col.Width

                            If e.X <= colEndX And e.X >= colEndX - col.Width Then
                                _hoveredCol = col
                                Exit For
                            End If
                        Next

                        If Not _hoveredCol Is Nothing AndAlso row > -1 AndAlso TypeOf (_hoveredCol.DataCell) Is KNActionCell Then

                            'We're over an action cell!
                            Dim cellRect As Rectangle = New Rectangle(colEndX - _hoveredCol.Width, _headerHeight + 1 + ((row - scroller.Value) * _defaultRowHeight), _
                            _hoveredCol.Width, _defaultRowHeight)
                            Dim shouldDraw As Boolean

                            _currentCellRect = cellRect
                            _currentCell = _hoveredCol.CellForRow(row)

                            shouldDraw = CType(_hoveredCol.CellForRow(row), KNActionCell).MouseDidMoveInCell(New Point(e.X - cellRect.X, e.Y - cellRect.Y), New Rectangle(0, 0, cellRect.Width, cellRect.Height))

                            If shouldDraw Then
                                MyBase.Invalidate(cellRect)
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    Private Shadows Sub MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseUp

        If Not _currentCell Is Nothing And _cellSwallowedMouse = True Then

            ' Cell swallows mouse.

            If TypeOf (_currentCell) Is KNActionCell Then
                If CType(_currentCell, KNActionCell).MouseUpInCell(New Point(e.X - _currentCellRect.X, e.Y - _currentCellRect.Y), _
                            New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height)) Then
                    MyBase.Invalidate(_currentCellRect)
                End If
            End If

            _cellSwallowedMouse = False
            Return
        End If

        If e.Location = _mouseDownPoint And e.Button = Windows.Forms.MouseButtons.Left Then

            If (_isWaitingForSecondClick) And e.Location = _firstClickPoint Then
                _isWaitingForSecondClick = False
                If _clickedRow < _dataSource.NumberOfItemsInTableView(Me) Then
                    RaiseEvent RowDoubleClicked(_clickedRow, _clickedColumn)
                End If

            Else
                _isWaitingForSecondClick = True
                _doubleClickTimer = New Timer()
                _doubleClickTimer.Interval = 500
                _doubleClickTimer.Start()
                _firstClickPoint = e.Location
            End If



        End If

        If Not _actionedColumn Is Nothing Then
            _actionedColumn.HeaderCell.State = KNCell.KNCellState.KNOffState
            MyBase.Invalidate(New Rectangle(0, 0, MyBase.Width, _headerHeight))

            If _dragAction = KNHeaderDragAction.NoAction Then
                If e.X >= _columnStartX And e.X <= _columnStartX + _actionedColumn.Width And e.Y >= 0 And e.Y <= _headerHeight Then
                    ' Over the actioned column

                    DealWithHeaderClicked(_actionedColumn)

                End If
            End If

        End If

        _dragAction = KNHeaderDragAction.NoAction
    End Sub

    Public Sub clickTimerFired(ByVal sender As Object, ByVal e As EventArgs) Handles _doubleClickTimer.Tick

        _doubleClickTimer.Stop()
        _doubleClickTimer.Enabled = False

        If (_isWaitingForSecondClick) Then
            _isWaitingForSecondClick = False
        End If
    End Sub

    Private Sub MouseLeft(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.MouseLeave
        If Not _currentCell Is Nothing Then
            If TypeOf (_currentCell) Is KNActionCell Then

                If CType(_currentCell, KNActionCell).MouseDidMoveInCell(New Point(-1, -1), New Rectangle(0, 0, _currentCellRect.Width, _currentCellRect.Height)) Then
                    MyBase.Invalidate(_currentCellRect)
                End If

            End If
        End If
    End Sub

    Private Sub DealWithHeaderClicked(ByRef column As KNTableColumn, Optional ByVal order As SortOrder = SortOrder.None)

        Dim newSortOrder As SortOrder

        If order = SortOrder.None Then

            If column Is PrimarySortColumn() Then
                If _sortOrder = SortOrder.Ascending Then
                    newSortOrder = SortOrder.Descending
                Else
                    newSortOrder = SortOrder.Ascending
                End If
            Else

                newSortOrder = SortOrder.Ascending

            End If

        Else
            newSortOrder = order
        End If

        If Not _delegate Is Nothing Then
            newSortOrder = _delegate.ColumnHeaderClicked(Me, column, newSortOrder)
        End If

        If newSortOrder = SortOrder.None Then
            ' Do nothing

        Else


            _sortOrder = newSortOrder

            For Each col As KNTableColumn In _columns

                If (col Is column) Then
                    col.SortingPriority = KNTableColumn.SortPriority.Primary
                Else
                    col.SortingPriority = KNTableColumn.SortPriority.NotUsed
                End If


            Next
            ReloadData()


        End If




    End Sub

    Public Sub SetSortColumn(ByRef column As KNTableColumn, ByVal order As SortOrder)
        DealWithHeaderClicked(column, order)

    End Sub

    Public Function PrimarySortColumn() As KNTableColumn
        For Each column As KNTableColumn In _columns
            If column.SortingPriority = KNTableColumn.SortPriority.Primary Then
                Return column
            End If
        Next

        Return Nothing
    End Function

    Public Function ColumnWithIdentifier(ByVal identifier As String) As KNTableColumn
        For Each col As KNTableColumn In _columns
            If col.Identifier = identifier Then
                Return col
            End If
        Next

        Return Nothing
    End Function

    Public Sub AssureRowIsVisible(ByVal row As Integer)

        If row < scroller.Value Then
            scroller.Value = row
            MyBase.Invalidate()
        End If

        If row >= (scroller.Value + scroller.LargeChange) Then
            scroller.Value = row - scroller.LargeChange
            MyBase.Invalidate()
        End If


    End Sub

    Public Sub ScrollToRow(ByVal row As Integer)

        Dim pos As Integer = 0

        If (row > scroller.Maximum - scroller.LargeChange) Then
            pos = (scroller.Maximum - scroller.LargeChange) + 1
        Else
            pos = row
        End If

        If pos < 0 Then
            pos = 0
        End If

        scroller.Value = pos

        MyBase.Invalidate()

    End Sub

    Public Sub SelectUpOneItem(ByVal hasShiftKey As Boolean)

        If Not Me.AllowsMultipleSelection Then
            hasShiftKey = False
        End If

        Dim minRow As Integer = DataSource.NumberOfItemsInTableView(Me)
        Dim maxRow As Integer = -1

        For Each index As Integer In SelectedRows
            If index < minRow Then
                minRow = index
            End If

            If index > maxRow Then
                maxRow = index
            End If
        Next

        If hasShiftKey Then

            _selectedRows.Clear()


            Dim row As Integer
            For row = minRow To maxRow
                If Not _delegate Is Nothing Then
                    If _delegate.ShouldSelectRow(Me, row) Then
                        _selectedRows.Add(row)
                    End If
                Else
                    _selectedRows.Add(row)
                End If
            Next
        End If


        minRow = minRow - 1

        If minRow < 1 Then
            minRow = 0 ' Top Row
        End If

        ' minRow is now our target selection.

        For minRow = minRow To 0 Step -1
            If Not _delegate Is Nothing Then
                If _delegate.ShouldSelectRow(Me, minRow) Then
                    If Not hasShiftKey Then
                        _selectedRows.Clear()
                    End If
                    _selectedRows.Add(minRow)
                    AssureRowIsVisible(minRow)
                    MyBase.Invalidate()
                    Exit For
                End If
            Else
                If Not hasShiftKey Then
                    _selectedRows.Clear()
                End If
                _selectedRows.Add(minRow)
                AssureRowIsVisible(minRow)
                MyBase.Invalidate()
                Exit For
            End If
        Next

        RaiseEvent SelectionChanged(_selectedRows)

    End Sub

    Public Sub SelectDownOneItem(ByVal hasShiftKey As Boolean)

        If Not Me.AllowsMultipleSelection Then
            hasShiftKey = False
        End If

        Dim count As Integer = DataSource.NumberOfItemsInTableView(Me)

        Dim minRow As Integer = count
        Dim maxRow As Integer = -1

        For Each index As Integer In SelectedRows
            If index < minRow Then
                minRow = index
            End If

            If index > maxRow Then
                maxRow = index
            End If
        Next

        If hasShiftKey Then
            Dim row As Integer

            _selectedRows.Clear()

            For row = minRow To maxRow
                If Not _delegate Is Nothing Then
                    If _delegate.ShouldSelectRow(Me, row) Then
                        _selectedRows.Add(row)
                    End If
                Else
                    _selectedRows.Add(row)
                End If
            Next
        End If

        maxRow = maxRow + 1

        If maxRow > count - 1 Then
            maxRow = count - 1 ' Bottom Row
        End If

        ' maxRow is now our target selection.

        For maxRow = maxRow To count - 1 Step 1
            If Not _delegate Is Nothing Then
                If _delegate.ShouldSelectRow(Me, maxRow) Then
                    If Not hasShiftKey Then
                        _selectedRows.Clear()
                    End If
                    _selectedRows.Add(maxRow)
                    AssureRowIsVisible(maxRow)
                    MyBase.Invalidate()
                    Exit For
                End If
            Else
                If Not hasShiftKey Then
                    _selectedRows.Clear()
                End If
                _selectedRows.Add(maxRow)
                AssureRowIsVisible(maxRow)
                MyBase.Invalidate()
                Exit For
            End If
        Next

        RaiseEvent SelectionChanged(_selectedRows)

    End Sub

    Private Function ColumnForX(ByVal X As Integer, Optional ByVal ExtraPadding As Integer = 0) As KNTableColumn
        Dim colEndX As Integer = 0

        For Each col As KNTableColumn In _columns
            colEndX += col.Width

            If X <= colEndX + ExtraPadding And X >= colEndX - col.Width Then
                Return col

            End If
        Next

        Return Nothing

    End Function




    Private Sub scrollerChanged(ByVal sender As Object, ByVal e As EventArgs) Handles scroller.ValueChanged
        MyBase.Invalidate()

    End Sub

    'Resize Methods

    Private Sub Resized(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.SizeChanged

        Dim rowsVisible As Integer = (MyBase.Height - _headerHeight - 2) / _defaultRowHeight
        If (rowsVisible > -1) Then
            scroller.LargeChange = rowsVisible
        End If

        ResizeHeadersUsingSetSizing()

        MyBase.Invalidate()


    End Sub

    Private Sub ResizeHeadersUsingSetSizing()

        If UBound(_columns) > -1 Then

            Select Case _headerSizing
                Case HeaderAutoSizing.DivideEqually

                    Dim columnsLeft As ArrayList = New ArrayList

                    columnsLeft.AddRange(_columns)

                    Dim fixedSizeColumns As ArrayList = New ArrayList
                    Dim widthLostToFixedColumns As Long = 0

                    For Each column As KNTableColumn In _columns
                        If column.MaximumSize = column.MinimumSize Then
                            fixedSizeColumns.Add(column)
                            widthLostToFixedColumns += column.MaximumSize
                            column.Width = column.MaximumSize
                            columnsLeft.Remove(column)
                        End If
                    Next

                    While columnsLeft.Count > 0

                        Dim allColumnsFit As Boolean = True
                        Dim suggestedHeaderWidth As Integer = (MyBase.Width - scroller.Width - widthLostToFixedColumns) / columnsLeft.Count
                        Dim columnsThatDidntFit As ArrayList = New ArrayList

                        For Each column As KNTableColumn In columnsLeft

                            If column.MinimumSize > suggestedHeaderWidth OrElse column.MaximumSize < suggestedHeaderWidth Then
                                columnsThatDidntFit.Add(column)
                                allColumnsFit = False
                            End If
                        Next


                        For Each column As KNTableColumn In columnsThatDidntFit
                            If suggestedHeaderWidth < column.MinimumSize Then
                                column.Width = column.MinimumSize
                            Else
                                column.Width = column.MaximumSize
                            End If

                            widthLostToFixedColumns += column.Width
                            fixedSizeColumns.Add(column)
                            columnsLeft.Remove(column)
                        Next

                        If allColumnsFit Then
                            For Each column As KNTableColumn In columnsLeft
                                column.Width = suggestedHeaderWidth
                                fixedSizeColumns.Add(column)
                                widthLostToFixedColumns += column.Width
                            Next

                            columnsLeft.RemoveRange(0, columnsLeft.Count)

                        End If


                    End While
            End Select

        End If

    End Sub


    ' Keyboard


    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        If keyData = (Keys.Up Or Keys.Shift) Then
            Me.SelectUpOneItem(True)
            Return True
        End If

        If keyData = (Keys.Up) Then
            Me.SelectUpOneItem(False)
            Return True
        End If

        If keyData = (Keys.Down Or Keys.Shift) Then
            Me.SelectDownOneItem(True)
            Return True
        End If

        If keyData = (Keys.Down) Then
            Me.SelectDownOneItem(False)
            Return True
        End If

        If keyData = (Keys.PageDown) Then
            If (scroller.Value + scroller.LargeChange) > ((scroller.Maximum - scroller.LargeChange) + 1) Then
                scroller.Value = (scroller.Maximum - scroller.LargeChange) + 1
            Else
                scroller.Value += scroller.LargeChange
            End If
            Return True
        End If

        If keyData = (Keys.PageUp) Then
            If (scroller.Value - scroller.LargeChange) < scroller.Minimum Then
                scroller.Value = scroller.Minimum
            Else
                scroller.Value -= scroller.LargeChange
            End If
            Return True
        End If

        If keyData = (Keys.Home) Then
            ScrollToRow(0)
            Return True
        End If

        If keyData = (Keys.End) Then
            ScrollToRow(scroller.Maximum + 1)
            Return True
        End If




        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Public Sub scrollerMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles scroller.MouseDown

        Me.Focus()

    End Sub


    Private Sub ActionCellPerformedAction(ByRef Cell As KNActionCell, ByRef Column As KNTableColumn) Implements KNTableColumnDelegate.ActionCellPerformedAction
        RaiseEvent CellPerformedAction(Me.RowForCellInColumn(Cell, Column), Column, Cell)
    End Sub

    Public Function RowForCellInColumn(ByRef cell As KNCell, ByRef column As KNTableColumn) As Integer
        Return column.RowForCell(cell)
    End Function

    ' Public Methods

    Public Sub ReloadData()
        MyBase.Invalidate()
        MyBase.Refresh()
    End Sub

    Public Sub ResetSelection()
        _selectedRows.Clear()
        MyBase.Invalidate()
    End Sub

    Public Sub SetSelectedRow(ByVal row As Integer)

        If Not _delegate Is Nothing Then

            If _delegate.ShouldSelectRow(Me, row) Then

                _selectedRows.Clear()
                If row > -1 And row < _dataSource.NumberOfItemsInTableView(Me) Then
                    _selectedRows.Add(row)
                End If

                MyBase.Invalidate()
                RaiseEvent SelectionChanged(_selectedRows)

            End If

        Else

            _selectedRows.Clear()
            If row > -1 And row < _dataSource.NumberOfItemsInTableView(Me) Then
                _selectedRows.Add(row)
            End If

            MyBase.Invalidate()
            RaiseEvent SelectionChanged(_selectedRows)
        End If
    End Sub


    Public Sub AddColumn(ByVal col As KNTableColumn)
        ReDim Preserve _columns(UBound(_columns) + 1)
        col.ColumnDelegate = Me
        _columns(UBound(_columns)) = col
        ResizeHeadersUsingSetSizing()
    End Sub

#Region "Properties"

    Public Property DefaultRowHeight() As Integer
        Get
            Return _defaultRowHeight
        End Get
        Set(ByVal value As Integer)
            _defaultRowHeight = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property DataSource() As KNTableViewDataSource
        Get
            Return _dataSource
        End Get
        Set(ByVal value As KNTableViewDataSource)
            _dataSource = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property AlternatingRowBackgrounds() As Boolean
        Get
            Return _alternatingRow
        End Get
        Set(ByVal value As Boolean)
            _alternatingRow = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property BackgroundColour() As Color
        Get
            Return _backgroundColor
        End Get
        Set(ByVal value As Color)
            _backgroundColor = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property AlternateBackgroundColour() As Color
        Get
            Return _alternateRowColor
        End Get
        Set(ByVal value As Color)
            _alternateRowColor = value
        End Set
    End Property

    Public Property DrawHorizontalGridLines() As Boolean
        Get
            Return _drawHorizontalGridLines
        End Get
        Set(ByVal value As Boolean)
            _drawHorizontalGridLines = value
            MyBase.Invalidate()

        End Set
    End Property

    Public Property DrawVerticalGridLines() As Boolean
        Get
            Return _drawVerticalGridLines
        End Get
        Set(ByVal value As Boolean)
            _drawVerticalGridLines = value
            MyBase.Invalidate()

        End Set
    End Property

    Public Property GridColour() As Color
        Get
            Return _gridColor
        End Get
        Set(ByVal value As Color)
            _gridColor = value
        End Set
    End Property

    Public Property CornerCell() As KNCell
        Get
            Return _cornerCell
        End Get
        Set(ByVal value As KNCell)
            _cornerCell = value
        End Set
    End Property

    Public Property HeaderSizingMode() As HeaderAutoSizing
        Get
            Return _headerSizing
        End Get
        Set(ByVal value As HeaderAutoSizing)
            _headerSizing = value

            Me.ResizeHeadersUsingSetSizing()

        End Set
    End Property

    Public Property SelectionStyling() As SelectionStyle
        Get
            Return _selectionStyle
        End Get
        Set(ByVal value As SelectionStyle)
            _selectionStyle = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property SelectedRows() As ArrayList
        Get
            Return _selectedRows
        End Get
        Set(ByVal value As ArrayList)
            _selectedRows = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property TableDelegate() As KNTableViewDelegate
        Get
            Return _delegate
        End Get
        Set(ByVal value As KNTableViewDelegate)
            _delegate = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property AutoHideScrollBars() As Boolean
        Get
            Return _autoHideScrollBars
        End Get
        Set(ByVal value As Boolean)
            _autoHideScrollBars = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property AllowsMultipleSelection() As Boolean
        Get
            Return _allowsMultipleSelection
        End Get
        Set(ByVal value As Boolean)
            _allowsMultipleSelection = value
        End Set
    End Property

    Public Property AllowEmptySelection() As Boolean
        Get
            Return _allowEmptySelection
        End Get
        Set(ByVal value As Boolean)
            _allowEmptySelection = value
        End Set
    End Property

    Public Property HeaderHeight() As Integer
        Get
            Return _headerHeight
        End Get
        Set(ByVal value As Integer)
            scroller.Top = value + 1
            scroller.Height = MyBase.Height - value - 1
            _headerHeight = value
            MyBase.Invalidate()
        End Set
    End Property

#End Region

    Private Declare Function GetSystemMetrics Lib "user32" (ByVal nIndex As Long) As Long
    Private Declare Function GetDoubleClickTime Lib "user32" () As Long
    Const SM_CXDOUBLECLK = 36
    Const SM_CYDOUBLECLK = 37

    ' retrieve info about the double-click area, and the
    ' time within which the user must click the mouse button
    ' again to be considered a double-clicl
    '
    ' WIDTH and HEIGHT are the size (in pixels) of the rectangle
    ' inside which the second click must occur - default is 4 pixels
    ' TIMEOUT is the timeout (in milliseconds) - default is 500 milliseconds.
    '
    ' Usage:
    '  Dim wi As Long, he As Long, ti As Long
    '  GetDoubleClickInfo wi, he, ti
    '  Print "Width=" & wi & ", Height=" & he & ", Timeout=" & ti

    Sub GetDoubleClickInfo(ByVal Width As Long, ByVal Height As Long, ByVal Timeout As Long)
        ' use GetSystemMetrics to retrieve the rectangle size
        Width = GetSystemMetrics(SM_CXDOUBLECLK)
        Height = GetSystemMetrics(SM_CYDOUBLECLK)
        ' use GetDoubleClickTime to retrieve the timeout
        Timeout = GetDoubleClickTime()
    End Sub

    Public Sub UpdateCell(ByRef cell As KNCell) Implements KNCellContainer.UpdateCell
        MyBase.Invalidate()
    End Sub
End Class





Public Interface KNTableViewDelegate

    Function ColumnHeaderClicked(ByRef tableView As KNTableView, ByRef column As KNTableColumn, ByVal suggestedNewSortOrder As SortOrder) As SortOrder
    Function ShouldSelectRow(ByRef tableView As KNTableView, ByVal rowIndex As Integer) As Boolean

End Interface


Public Interface KNTableViewDataSource

    Function NumberOfItemsInTableView(ByRef tableView As KNTableView) As Integer
    Function ObjectForRow(ByRef tableView As KNTableView, ByRef tableColumn As KNTableColumn, ByVal rowIndex As Integer) As Object

End Interface


