Imports System.Collections.Generic

Public Class KNNotification

    Private _notificationName As String
    Private _sender As Object
    Private _properties As Dictionary(Of String, Object)


    Public Sub New(ByVal name As String, ByVal sender As Object, ByVal properties As Dictionary(Of String, Object))
        _sender = sender
        _notificationName = name
        If Not properties Is Nothing Then
            _properties = properties
        Else
            ' Just so calls to .Property won't thow exceptions if no
            ' properties are given.
            _properties = New Dictionary(Of String, Object)
        End If
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return _notificationName
        End Get
    End Property

    Public ReadOnly Property Sender() As Object
        Get
            Return _sender
        End Get
    End Property

    Public ReadOnly Property Properties() As Dictionary(Of String, Object)
        Get
            Return _properties
        End Get
    End Property


End Class
