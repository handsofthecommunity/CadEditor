using CadEditor;
using System;
using System.Drawing;
//css_include settings_rockin_cats/RockinCats-Utils.cs;

public class Data : RockinCatsBase
{
  public override OffsetRec getScreensOffset()  { return new OffsetRec(0x14a82, 29 , 3*2, 3, 2);    }
  public override int getVideoIndex1()          { return 0x14; }
  public override int getVideoIndex2()          { return 0x16; }
  public override OffsetRec getBlocksOffset()   { return new OffsetRec(0x1454b ,1  , 0x4000); }
  public override int getBlocksCount()          { return 83; }
  
  public override OffsetRec getBigBlocksOffsetHierarchy(int hierarchyLevel)
  { 
    if (hierarchyLevel == 0) { return new OffsetRec(0x146ea, 1  , 0x4000); }
    if (hierarchyLevel == 1) { return new OffsetRec(0x148ee, 1  , 0x4000); }
    if (hierarchyLevel == 2) { return new OffsetRec(0x14a12, 1  , 0x4000); }
    return new OffsetRec(0x0, 1  , 0x4000);
  }
  
  public override int getBigBlocksCountHierarchy(int hierarchyLevel)
  { 
    if (hierarchyLevel == 0) { return 129; }
    if (hierarchyLevel == 1) { return 73; }
    if (hierarchyLevel == 2) { return 56;  }
    return 256;
  }
  
  public override byte[] getPallete(int palId)
  {
    var pallete = new byte[] { 
      0x0f, 0x16, 0x28, 0x30, 0x0f, 0x12, 0x18, 0x28,
      0x0f, 0x08, 0x17, 0x27, 0x0f, 0x1a, 0x17, 0x2a,
    }; 
    return pallete;
  }
}