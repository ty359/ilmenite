// 水流试验demo
// 用于建立水流模型

#include "pxyz.h"
#include "chunk.h"

#include <bits/stdc++.h>

using namespace ILMENITE;
using namespace std;

int WaterFlat(Chunk *c) {
    int tot_delta = 0;
    for (auto p: Chunk::Range()) {
        MetaBlock &b = c->b_map[p.x][p.y][p.z];
        if (b.id == MetaBlock::E_WATER) {
            b.info.water.delta = rand_div(b.info.water.delta, 2);
            int dd = 0;
            for (auto dirc: Pxyz::Direction) {
                const MetaBlock &bb = c->GetBlock(p + dirc);
                if (bb.id == MetaBlock::E_WATER) {
                    dd += bb.info.water.getlevel();
                }
                else {
                    dd += b.info.water.getlevel();
                }
            }
            b.info.water.delta += rand_div(dd, 4) - b.info.water.getlevel();
            int new_level = b.info.water.getlevel() + b.info.water.delta;

            // 水位溢出逻辑
            if (new_level >= 0x100) {
                fprintf(stderr, "warn: clide!\n");
                b.info.water.delta -= new_level - 0x100;
                b.info.water.level = 0;
            }
            else if (new_level <= 0) {
                fprintf(stderr, "warn: clide!\n");
                b.info.water.delta -= new_level - 1;
                b.info.water.level = 1;
            }
            else {
                b.info.water.level = new_level;
            }

            tot_delta += b.info.water.delta;
        }
    }
    return tot_delta;
}

// TODO: 竖直方向上的水流模拟
// TODO: 建立模型以解决在计算中由于整数误差导致的水量错误
// TODO: 图形化展示

int main() {
    printf("%d\n", -1 / 4);

    srand(time(NULL));

    sizeof(MetaBlock);
    Chunk c;

    int i = 0;
    for (auto pxyz: Chunk::Range()) {
        i += 1;
    }
    printf("%d\n", i);

    for (auto p: Chunk::Range(1)) {
        c.b_map[p.x][p.y][p.z].id = MetaBlock::E_WATER;
        c.b_map[p.x][p.y][p.z].info.water.level = 
            p.x < 10 ? (50 + rand() % 150) : 30;
    }
    while (0) {
        int x, y;
        scanf("%d%d", &x, &y);
        printf("%d\n", rand_div(x, y));
    }

    int64_t final_tot_delta = 0;
    for (int i = 0; i < 500; ++ i) {
        int tot_delta = WaterFlat(&c);
        final_tot_delta += tot_delta;
        int tot_level = 0;
        int max_level = 0;
        int min_level = 999;
        for (auto p: Chunk::Range(1)) {
            tot_level += c.b_map[p.x][p.y][p.z].info.water.getlevel();
            max_level = max(max_level, c.b_map[p.x][p.y][p.z].info.water.getlevel());
            min_level = min(min_level, c.b_map[p.x][p.y][p.z].info.water.getlevel());
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