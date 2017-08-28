namespace nullEngine
{
    //this is a class to store all of the necessary texture data
    public class Texture2D
    {
        private int _id;
        private int _width, _height;
        private float _xStart, _yStart, _xEnd, _yEnd;

        public int id { get { return _id; } }
        public int width { get { return _width; } }
        public int height { get { return _height; } }

        public float xStart { get { return _xStart; } }
        public float xEnd { get { return _xEnd; } }
        public float yStart { get { return _yStart; } }
        public float yEnd { get { return _yEnd; } }

        public Texture2D(int id, int width, int height)
        {
            _id = id;
            _width = width;
            _height = height;
            _xStart = 0;
            _xEnd = 1;
            _yStart = 0;
            _yEnd = 1;
        }

        public Texture2D(int id, int width, int height, float xStart, float yStart, float xEnd, float yEnd)
        {
            _id = id;
            _width = width;
            _height = height;
            _xStart = xStart;
            _xEnd = xEnd;
            _yStart = yStart;
            _yEnd = yEnd;
        }
    }
}
