Imports System.Windows.Forms

Public Class KNEditableCell
    Inherits KNActionCell

    Private Shared _isEditing As Boolean
    Private Shared _currentHandler As KNEditableCell
    Private Shared _sharedEditor As TextBox = New TextBox()

    Public Shared ReadOnly Property SharedTextEditor() As TextBox
        Get
            Return _sharedEditor
        End Get
    End Property



    Protected Sub BeginTextEditingInFrame(ByVal frame As Rectangle)

        If _isEditing Then
            EndTextEditing()
        End If

        Dim textEditor As TextBox = SharedTextEditor

        AdjustEditorIfNeeded(frame)
        textEditor.Text = Me.ObjectValue
        textEditor.BorderStyle = BorderStyle.None
        textEditor.Visible = True
        textEditor.Margin = New Padding(0)
        textEditor.SelectAll()
        _isEditing = True
        _currentHandler = Me
        Me.ParentControl.Control.Controls.Add(textEditor)
        ApplyHandlersToSharedEditor()
        textEditor.Focus()

    End Sub

    Public Sub EndTextEditing()

        _isEditing = False
        SharedTextEditor.Visible = False
        RemoveHandlersFromSharedEditor()
        _currentHandler = Nothing
        Me.ParentControl.Control.Controls.Remove(SharedTextEditor)
        HasCompletedEditing(SharedTextEditor.Text)
        SharedTextEditor.Text = ""
        Me.ParentControl.Control.Focus()



    End Sub

    Private Shared Sub ApplyHandlersToSharedEditor()
        'We want... 
        ' - TextChanged
        ' - KeyPressed (for return, enter, delete)
        ' - LostFocus

        AddHandler SharedTextEditor.TextChanged, AddressOf _currentHandler.TextChanged
        AddHandler SharedTextEditor.LostFocus, AddressOf _currentHandler.LostFocus
        AddHandler SharedTextEditor.KeyPress, AddressOf _currentHandler.KeyPress



    End Sub

    Private Shared Sub RemoveHandlersFromSharedEditor()

        RemoveHandler SharedTextEditor.TextChanged, AddressOf _currentHandler.TextChanged
        RemoveHandler SharedTextEditor.LostFocus, AddressOf _currentHandler.LostFocus
        RemoveHandler SharedTextEditor.KeyPress, AddressOf _currentHandler.KeyPress

    End Sub



    Private Sub TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        ValueDidChange(SharedTextEditor.Text)
    End Sub

    Private Sub LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        EndTextEditing()
    End Sub

    Private Sub KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        KeyWasPressed(e.KeyChar)
    End Sub

    Public Shared ReadOnly Property isEdting() As Boolean
        Get
            Return _isEditing
        End Get
    End Property

    Public Shared ReadOnly Property CurrentEditingCell() As KNEditableCell
        Get
            Return _currentHandler
        End Get
    End Property

    Public Overrides Property Highlighted() As Boolean
        Get
            Return MyBase.Highlighted
        End Get
        Set(ByVal value As Boolean)

            If value = False AndAlso ReferenceEquals(Me, CurrentEditingCell) Then
                Me.EndTextEditing()
            End If

            MyBase.Highlighted = value
        End Set
    End Property

    Public Sub AdjustEditorIfNeeded(frame as Rectangle)
        SharedTextEditor.SetBounds(frame.X + 8, frame.Y + 1, frame.Width - 8, frame.Height - 2)
    End Sub


#Region "Stuff to override"

    Public Overridable Sub ValueDidChange(ByVal value As Object)

        Me.ObjectValue = value

    End Sub

    Public Overridable Sub HasCompletedEditing(ByVal value As Object)

        Me.ObjectValue = value

        If Not CellDelegate Is Nothing Then
            CellDelegate.CellPerformedAction(Me)
        End If

    End Sub

    Public Overridable Sub KeyWasPressed(ByVal keyChar As Char)

        If AscW(keyChar) = Keys.Return Or AscW(keyChar) = Keys.Enter Then
            EndTextEditing()
        End If

    End Sub


#End Region


End Class
