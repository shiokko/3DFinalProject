// for passing between item controller and raycast hitter
enum Items
{
    CHARM = 1,  // because lantern is indexas 0
    DIVINATION_BLOCK,
    INCENSE,
    WOOD_SWORD
}

// for passing between player and god
enum Remnants
{
    // for sex
    COMB = 0,
    FAN,

    // for age
    TOY,
    JADE,
    CRUTCH, 

    // for hierarchy
    GOLD,
    HAT,
    BOWL,
}

enum Gods
{
    LAND = 0,  // no relted to ghost type

    CHANG_HOUNG,
    DI_ZHONG
}

enum GlobalVar
{
    NUM_REMNANT_TYPE = 8,
    NUM_REMNANT_CATEGORY = 3,
    NUM_ITEM_TYPE = 5,
    NUM_GHOST_TYPE = 2,
}