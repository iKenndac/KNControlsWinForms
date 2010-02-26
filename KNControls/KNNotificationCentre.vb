Imports System.Collections.Generic

Public Class KNNotificationCentre

    Private Shared _sharedCentre As KNNotificationCentre

    Shared Function DefaultNotificationCentre() As KNNotificationCentre
        If _sharedCentre Is Nothing Then
            _sharedCentre = New KNNotificationCentre
        End If

        Return _sharedCentre
    End Function

    ' This is a dictionary of Notification names to delegates interested 
    ' in that notification. 

    Private _delegates As Dictionary(Of String, ArrayList)


    Public Sub New()
        _delegates = New Dictionary(Of String, ArrayList)
    End Sub

    Public Sub AddObserverForNotificationName(ByRef del As KNNotificationDelegate, ByVal notificationName As String)

        ' Add the delegate to the list
        If Not _delegates.ContainsKey(notificationName) Then
            _delegates.Add(notificationName, New ArrayList)
        End If

        If Not _delegates.Item(notificationName).Contains(del) Then
            _delegates.Item(notificationName).Add(del)
        End If

    End Sub

    Public Sub RemoveNotificationDelegate(ByRef del As KNNotificationDelegate)

        For Each listOfDelegates As ArrayList In _delegates.Values
            If listOfDelegates.Contains(del) Then
                listOfDelegates.Remove(del)
            End If
        Next

    End Sub

    Public Sub RemoveObserver(ByRef observer As Object)

        Dim itemsToRemove As New ArrayList

        For Each listOfDelegates As ArrayList In _delegates.Values

            For Each notificationDel As KNNotificationDelegate In listOfDelegates

                If ReferenceEquals(notificationDel.Target, observer) Then
                    itemsToRemove.Add(notificationDel)
                End If

            Next

            For Each removalDel As KNNotificationDelegate In itemsToRemove
                listOfDelegates.Remove(removalDel)
            Next

            itemsToRemove.Clear()

        Next

    End Sub

#Region "Posting"

    Public Sub PostNotification(ByVal notification As KNNotification)
        If _delegates.ContainsKey(notification.Name) Then

            Dim listOfDelegates As ArrayList = _delegates.Item(notification.Name)

            For Each del As KNNotificationDelegate In listOfDelegates
                If Not del Is Nothing AndAlso Not del.Target Is Nothing Then
                    del.Invoke(notification)
                End If

            Next


        End If
    End Sub


    Public Sub PostNotificationWithName(ByVal name As String, ByVal sender As Object)
        PostNotification(New KNNotification(name, sender, Nothing))
    End Sub

    Public Sub PostNotificationWithName(ByVal name As String, ByVal sender As Object, ByVal props As Dictionary(Of String, Object))
        PostNotification(New KNNotification(name, sender, props))
    End Sub

#End Region

End Class


Public Delegate Sub KNNotificationDelegate(ByRef Notification As KNNotification)
