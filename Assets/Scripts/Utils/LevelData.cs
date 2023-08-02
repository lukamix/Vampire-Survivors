public static class LevelData
{
    private static string[,] _levelData;
    public static string[,] levelData()
    {
        if (_levelData == null)
        {
            _levelData = new string[3, 2];
            _levelData[0, 0] = "Easy";
            _levelData[0, 1] = "Easy to win !";
            _levelData[1, 0] = "Normal";
            _levelData[1, 1] = "A little bit harder !";
            _levelData[2, 0] = "Hard";
            _levelData[2, 1] = "Not so hard !";
        }
        return _levelData;
    }
}
