#pragma once

#include <vector>
#include <string>
#include <cstdlib>
#include <cstring>

#include "pxyz.h"

namespace ILMENITE {

using std::string;
using std::vector;

#pragma pack(1)
struct MetaBlock {
    uint8_t id;
    union {
        uint8_t data[3];
        struct {
            uint8_t level;
            int16_t delta;
            inline int getlevel() const {return level? level : 0x100;}
        } water;
    } info;

    enum {
        E_VOID = 0,
        E_AIR,
        E_WATER
    };

    static const MetaBlock Void;
    static const MetaBlock Air;
};
#pragma pack()


struct Chunk {
    const static int X_SIZE = 64;
    const static int Y_SIZE = 64;
    const static int Z_SIZE = 16;

    Chunk() {
        memset(b_map, 0, sizeof(b_map));
    }

    MetaBlock b_map[X_SIZE][Y_SIZE][Z_SIZE];

    inline const MetaBlock& GetBlock(const Pxyz& p) {
        if (
            p.x >= 0 && p.x < X_SIZE &&
            p.y >= 0 && p.y < Y_SIZE &&
            p.z >= 0 && p.z < Z_SIZE
        ) {
            return b_map[p.x][p.y][p.z];
        }
        else {
            return MetaBlock::Void;
        }
    }

    class _Iter {
    public:
        _Iter(Pxyz xyz): _xyz(xyz) {}
        bool operator!= (const _Iter& other) {
            return _xyz != other._xyz;
        }
        const Pxyz& operator*() {
            return _xyz;
        }
        _Iter& operator++() {
            _xyz += {1, 0, 0};
            if (_xyz.x == X_SIZE) {
                _xyz += {-X_SIZE, 1, 0};
            }
            if (_xyz.y == Y_SIZE) {
                _xyz += {0, -Y_SIZE, 1};
            }
            return *this;
        }
    private:
        Pxyz _xyz;
    };

    class Range {
    public:
        Range(): _begin({0, 0, 0}), _end({0, 0, Z_SIZE}) {}
        Range(int h): _begin({0, 0, 0}), _end({0, 0, h}) {}
        _Iter &begin() {
            return _begin;
        }
        _Iter &end() {
            return _end;
        }
    private:
        _Iter _begin, _end;
    };
};

}