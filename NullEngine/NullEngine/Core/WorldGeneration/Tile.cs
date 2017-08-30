using System;

namespace NullEngine.WorldGen
{
    //this is a struct for each tile
    [Serializable]
    public struct Tile
    {
        public int TexID;
        [NonSerialized]
        public TextureAtlas tAtlas;
    }

    [Serializable]
    public struct worldTile
    {
        public Tile graphics;
        public string type;
        public bool isCollideable;
        public bool isContainer;
        public bool isRoad;
    }


    //
    //   0 1 2
    //   3 4 5
    //   6 7 8
    //
    public struct nineSplice
    {
        public bool Active_0;
        public worldTile tile_0;

        public bool Active_1;
        public worldTile tile_1;

        public bool Active_2;
        public worldTile tile_2;

        public bool Active_3;
        public worldTile tile_3;

        public bool Active_4;
        public worldTile tile_4;

        public bool Active_5;
        public worldTile tile_5;

        public bool Active_6;
        public worldTile tile_6;

        public bool Active_7;
        public worldTile tile_7;

        public bool Active_8;
        public worldTile tile_8;

        public worldTile CenterTile;

    }

    //this is an enum for all of the letters supported by /Content/font.png
    public enum letter
    {
        space = 0,
        exclaim = 1,
        double_quote = 2,
        hash = 3,
        dollar = 4,
        percent = 5,
        ampersand = 6,
        single_quote = 7,
        open_paren = 8,
        close_paren = 9,
        star = 10,
        plus = 11,
        comma = 12,
        dash = 13,
        period = 14,
        forward_slash = 15,
        zero = 16,
        one = 17,
        two = 18,
        three = 19,
        four = 20,
        five = 21,
        six = 22, 
        seven = 23,
        eight = 24,
        nine = 25,
        colon = 26,
        semicolon = 27,
        less_than = 28,
        equals = 29,
        greater_than = 30,
        question = 31,
        at = 32,
        A = 33,
        B = 34,
        C = 35,
        D = 36,
        E = 37,
        F = 38,
        G = 39,
        H = 40,
        I = 41,
        J = 42, 
        K = 43,
        L = 44,
        M = 45,
        N = 46,
        O = 47,
        P = 48,
        Q = 49,
        R = 50,
        S = 51,
        T = 52,
        U = 53,
        V = 54,
        W = 55,
        X = 56,
        Y = 57,
        Z = 58,
        open_bracket = 59,
        back_slash = 60,
        close_bracket = 61,
        carat = 62,
        underscore = 63,
        tick = 64,
        a = 65,
        b = 66,
        c = 67,
        d = 68,
        e = 69,
        f = 70,
        g = 71,
        h = 72,
        i = 73,
        j = 74,
        k = 75,
        l = 76,
        m = 77,
        n = 78,
        o = 79,
        p = 80,
        q = 81,
        r = 82,
        s = 83,
        t = 84,
        u = 85,
        v = 86,
        w = 87,
        x = 88,
        y = 89,
        z = 90,
        open_brace = 91,
        pipe = 92,
        close_brace = 93,
        tilde = 94,
        empty = 95
    }
}
