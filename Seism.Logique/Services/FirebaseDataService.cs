using Firebase.Database;
using Seism.Logique.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seism.Logique.Services;

public class FirebaseDataService : IDisposable
{
    private const string FirebaseUrl = "https://regidesolyd-default-rtdb.firebaseio.com/";
    private const string AuthToken = "zCa0CiNDACykrEpUPx9ZLNvRgfuIpFi0ZJvmMkmc";

    private FirebaseClient _firebaseClient;
    private IDisposable _subscription;

    public event EventHandler<AlertData> OnDataChanged;

    public FirebaseDataService()
    {
        _firebaseClient = new FirebaseClient(
            FirebaseUrl,
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(AuthToken)
            });
    }

    public void ListenForChanges()
    {
        _subscription = _firebaseClient
            .Child("")
            .AsObservable<AlertData>()
            .Subscribe(
                item => OnDataChanged?.Invoke(this, item.Object),
                ex => Console.WriteLine($"Error: {ex.Message}")
            );
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    public static async Task<List<AlertData>> GetAllAlert()
    {
        try
        {
            var firebaseClient = new FirebaseClient(
                FirebaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(AuthToken)
                });

            var data = await firebaseClient
                .Child("")
                .OnceAsync<AlertData>();

            return data
            .Select(item => new AlertData(item.Object))
            .ToList(); ;
        }
        catch (Exception ex)
        {
            return new();
        }
    }

}
