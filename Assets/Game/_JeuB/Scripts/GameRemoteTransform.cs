public class GameRemoteTransform : RemoteTransform
{
    public static GameRemoteTransform Instance;

    private void Awake()
    {
        Instance = this;
    }
}
