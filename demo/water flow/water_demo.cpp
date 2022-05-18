// 水流试验demo
// 用于建立水流模型

#include "pxyz.h"
#include "chunk.h"

#include <string>
#include <cstdlib>
#include <cstring>
#include <cctype>
#include <ctime>

using namespace ILMENITE;
using namespace std;

int WaterFlat(Chunk *c) {
    int tot_delta = 0;
    for (auto p: Chunk::Range()) {
        MetaBlock &b = c->b_map[p.x][p.y][p.z];
        if (b.id == MetaBlock::E_WATER) {
            int dd = 0;
            for (auto dirc: Pxyz::Direction) {
                const MetaBlock &bb = c->GetBlock(p + dirc);
                if (bb.id == MetaBlock::E_WATER) {
                    dd += bb.info.water.level;
                }
                else {
                    dd += b.info.water.level;
                }
            }
            dd -= (int)4 * b.info.water.level;

            {
                const MetaBlock &bup = c->GetBlock(p + Pxyz::Up);
                if (bup.id == MetaBlock::E_WATER && b.info.water.level != 0xff) {
                    dd += (int)4 * bup.info.water.level;
                }
                const MetaBlock &bdown = c->GetBlock(p + Pxyz::Down);
                if (bdown.id == MetaBlock::E_WATER && bdown.info.water.level != 0xff) {
                    dd -= (int)4 * b.info.water.level;
                }
            }
            
            dd = rand_div(dd, 4) + b.info.water.delta;
            if (dd < INT16_MIN) {
                b.info.water.delta = INT16_MIN;
            }
            else if (dd > INT16_MAX) {
                b.info.water.delta = INT16_MAX;
            }
            else {
                b.info.water.delta = dd;
            }
        }
    }

    vector<Pxyz> que;
    for (auto p: Chunk::Range()) {
        MetaBlock &b = c->b_map[p.x][p.y][p.z];
        if (b.id == MetaBlock::E_WATER) {
            int new_level = (int)b.info.water.level + b.info.water.delta;

            // 水位溢出逻辑
            if (new_level >= 0x100 || new_level < 0) {
                que.push_back(p);
            }
        }
    }

    for (int i = 0; i < (int)que.size(); ++ i) {
        Pxyz &p = que[i];
        MetaBlock &b = c->b_map[p.x][p.y][p.z];
        int new_level = (int)b.info.water.level + b.info.water.delta;
        if (new_level >= 0x100) {
            if (p.z + 1 < Chunk::Z_SIZE && c->b_map[p.x][p.y][p.z + 1].id == MetaBlock::E_WATER) {
                MetaBlock &bb = c->b_map[p.x][p.y][p.z + 1];
                bb.info.water.delta += new_level - 0xff;
                b.info.water.delta = 0xff - b.info.water.level;
                que.push_back({p.x, p.y, p.z + 1});
            }
        }
        else if (new_level < 0) {
            if (p.z - 1 >= 0 && c->b_map[p.x][p.y][p.z - 1].id == MetaBlock::E_WATER) {
                MetaBlock &bb = c->b_map[p.x][p.y][p.z - 1];
                bb.info.water.delta += new_level;
                b.info.water.delta = -(int)b.info.water.level;
                que.push_back({p.x, p.y, p.z - 1});
            }
        }
    }

    for (auto p: Chunk::Range()) {
        MetaBlock &b = c->b_map[p.x][p.y][p.z];
        if (b.id == MetaBlock::E_WATER) {
            int new_level = (int)b.info.water.level + b.info.water.delta;

            if (new_level >= 0x100 || new_level < 0) {
                fprintf(stderr, "flow error!\n");
            }
            else {
                b.info.water.level = new_level;
            }

            tot_delta += b.info.water.delta;
            b.info.water.delta = rand_div((int)b.info.water.delta * 7, 8);
        }
    }

    return tot_delta;
}

// TODO: 图形化展示
// TODO: 竖直方向上的水流模拟
// TODO: 建立模型以解决在计算中由于整数误差导致的水量错误
// TODO: 引入其他方块，如岩石、泥土的水模型

int main() {
    printf("%d\n", -1 / 4);

    srand(time(NULL));

    sizeof(MetaBlock);
    Chunk c;

    for (auto p: Chunk::Range(8)) {
        c.b_map[p.x][p.y][p.z].id = MetaBlock::E_WATER;
        c.b_map[p.x][p.y][p.z].info.water.level = 
            p.x < 10 ? (50 + rand() % 150) : 30;
    }
    for (auto p: Chunk::Range(1)) {
        c.b_map[p.x][p.y][p.z].info.water.level = 250;
    }

    int64_t final_tot_delta = 0;
    for (int i = 0; i < 500; ++ i) {
        int tot_delta = WaterFlat(&c);
        final_tot_delta += tot_delta;
        int tot_level = 0;
        int max_level = 0;
        int min_level = 999;
        for (auto p: Chunk::Range(2, 3)) {
            tot_level += c.b_map[p.x][p.y][p.z].info.water.level;
            max_level = max(max_level, (int)c.b_map[p.x][p.y][p.z].info.water.level);
            min_level = min(min_level, (int)c.b_map[p.x][p.y][p.z].info.water.level);
        }
        printf("tot_delta: %.3f,\tavg_level: %.3lf,\tmax_level: %.3lf,\tmin_level: %.3lf\n",
            (double)tot_delta / 64 / 64,
            (double)tot_level / 64 / 64,
            (double)max_level,
            (double)min_level);
    }
    printf("final_tot_delta: %.3lf\n", (double)final_tot_delta / 64 / 64);

    return 0;
}