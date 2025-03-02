using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InternetImage : MonoBehaviour
{
    [SerializeField] private Image _image;

    public static Dictionary<string, Sprite> _cachedImages = new();
    public static HashSet<string> _loadingImages = new();

    [ContextMenu("Test")]
    public void Test()
    {
        Load("https://i.ebayimg.com/images/g/Hs0AAeSwzntnp8Vm/s-l960.jpg");
    }

    public async void Load(string url)
    {
        await LoadAsync(url);
    }

    public async Task LoadAsync(string url)
    {
        if (_cachedImages.TryGetValue(url, out Sprite sprite))
        {
            _image.sprite = sprite;
            return;
        }

        if (_loadingImages.Contains(url))
        {
            while (_loadingImages.Contains(url))
            {
                await Task.Delay(100);
            }

            if (_cachedImages.TryGetValue(url, out sprite))
            {
                _image.sprite = sprite;
                return;
            }
        }

        _loadingImages.Add(url);
        using var www = UnityWebRequestTexture.GetTexture(url);
        await www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            var texture = DownloadHandlerTexture.GetContent(www);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
            _image.sprite = sprite;
            _cachedImages[url] = sprite;
        }
        _loadingImages.Remove(url);
    }
}