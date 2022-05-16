#pragma once

#include <vector>
#include <string>

namespace ILMENITE {

using std::string;
using std::vector;

struct Pxyz {
    int x, y, z;

    bool operator== (const Pxyz& other) const {
        return x == other.x && y == other.y && z == other.z;
    }
    bool operator!= (const Pxyz& other) const {
        return !(*this == other);
    }
    Pxyz &operator+= (const Pxyz& other) {
        x += other.x;
        y += other.y;
        z += other.z;
        return *this;
    }
    Pxyz operator+ (const Pxyz& other) const {
        Pxyz ret(*this);
        return ret += other;
    }

    string ToString() const {
        return (string)"(" + std::to_string(x) + ", " + std::to_string(y) + ", " + std::to_string(z) + ")";
    }

    const static Pxyz Up, Down, East, West, North, South;
    const static vector<Pxyz> Direction;
};

inline int rand_div(int x, int y) {
    //fprintf(stderr, "rand_div(%d, %d) : x/y = %d\n", x, y, x/y);
    int t = x % y;
    int neg = (t < 0);
    int ret = x / y;
    if (neg) {
        t = -t;
    }
    if (t == 0) {
        return ret;
    }
    return ret + ((rand() % y < t) ? (neg ? -1 : 1) : 0);
}

}