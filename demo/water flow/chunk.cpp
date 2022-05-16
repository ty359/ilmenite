#include "pxyz.h"
#include "chunk.h"

namespace ILMENITE {


const Pxyz Pxyz::Up     = { 0, 0, 1};
const Pxyz Pxyz::Down   = { 0, 0,-1};
const Pxyz Pxyz::East   = { 0, 1, 0};
const Pxyz Pxyz::South  = { 1, 0, 0};
const Pxyz Pxyz::West   = { 0,-1, 0};
const Pxyz Pxyz::North  = {-1, 0, 0};

const vector<Pxyz> Pxyz::Direction = {Pxyz::East, Pxyz::South, Pxyz::West, Pxyz::North};


const MetaBlock MetaBlock::Void = {MetaBlock::E_VOID, {0, 0, 0}};

}