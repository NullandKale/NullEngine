using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine
{
    public enum WorldTexID
    {
        oceanToGrassTopLeft = 0,
        oceanToGrassTopMid = 1,
        oceanToGrassTopRight = 2,
        oceanToGrassMidLeft = 21,
        oceanToGrassCenter = 22,
        oceanToGrassMidRight = 23,
        oceanToGrassBottomLeft = 42,
        oceanToGrassBottomMid = 43,
        oceanToGrassBottomRight = 44,

        grassToWaterTopLeft = 3,
        grassToWaterTopRight = 4,
        grassToWaterBottomLeft = 24,
        grassToWaterBottomRight = 25,

        grass0 = 45,
        grass1 = 46,
        grass2 = 22,

        grassToSandTopLeft = 63,
        grassToSandTopMid = 64,
        grassToSandTopRight = 65,
        grassToSandMidLeft = 84,
        grassToSandCenter = 85,
        grassToSandMidRight = 86,
        grassToSandBottomLeft = 105,
        grassToSandBottomMid = 106,
        grassToSandBottomRight = 107,

        beachToGrassTopLeft = 66,
        beachToGrassTopRight = 67,
        beachToGrassBottomLeft = 87,
        beachToGrassBottomRight = 88,

        beachInteriorTopLeft = 68,
        beachInteriorTopRight = 69,
        beachInteriorBottomLeft = 89,
        beachInteriorBottomRight = 90,

        beachTopLeft = 5,
        beachTopMid = 6,
        beachTopRight = 7,
        beachMidLeft = 26,
        beachCenter = 27,
        beachMidRight = 28,
        beachBottomLeft = 47,
        beachBottomMid = 48,
        beachBottomRight = 49,

        sandToWaterTopLeft = 129,
        sandToWaterTopRight = 130,
        sandToWaterBottomLeft = 150,
        sandToWaterBottomRight = 151,

        sandToGrassTopLeft = 66,
        sandToGrassTopRight = 67,
        sandToGrassBottomLeft = 87,
        sandToGrassBottomRight = 88,

        oceanToSandTopLeft = 126,
        oceanToSandTopMid = 127,
        oceanToSandTopRight = 128,
        oceanToSandMidLeft = 147,
        oceanToSandCenter = 148,
        oceanToSandMidRight = 149,
        oceanToSandBottomLeft = 168,
        oceanToSandBottomMid = 169,
        oceanToSandBottomRight = 170,

        sand0 = 27,
        sand1 = 108,
        sand2 = 109,
        sand3 = 85,
        sand4 = 148,

        water0 = 70,
        water1 = 171,
        water2 = 172,

        tree0 = 91,
        tree1 = 112,

        bridgeWestEnd = 10,
        bridgeEastWestMid = 11,
        bridgeEastEnt = 12,
        stairsUp = 13,
        stairsDown = 14,

        air = 188,
        wall = 75,
    }
}
