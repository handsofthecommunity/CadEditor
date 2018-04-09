using CadEditor;
using System;
using System.Collections.Generic;
using System.Drawing;

public class Data
{ 
  public OffsetRec getScreensOffset()     { return new OffsetRec(0x14025, 1, 16*96);  }
  public int getScreenWidth()    { return 16; }
  public int getScreenHeight()   { return 96; }
  
  public GetBigTileNoFromScreenFunc getBigTileNoFromScreenFunc() { return getBigTileNoFromScreen; }
  public SetBigTileToScreenFunc     setBigTileToScreenFunc()     { return setBigTileToScreen; }
  
  public bool isBuildScreenFromSmallBlocks() { return true; }
  
  public bool isBigBlockEditorEnabled() { return false; }
  public bool isBlockEditorEnabled()    { return true; }
  public bool isEnemyEditorEnabled()    { return false; }
  
  public GetVideoPageAddrFunc getVideoPageAddrFunc() { return getVideoAddress; }
  public GetVideoChunkFunc    getVideoChunkFunc()    { return getVideoChunk;   }
  public SetVideoChunkFunc    setVideoChunkFunc()    { return null; }
  
  public OffsetRec getBigBlocksOffset() { return new OffsetRec(0x14625  , 1  , 0x1000);  }
  public OffsetRec getBlocksOffset()    { return new OffsetRec(0x14625  , 1  , 0x1000);  }
  public int getBlocksCount()           { return 125; }
  public int getBigBlocksCount()        { return 125; }
  public int getPalBytesAddr()          { return 0x14625 + 16*125; }
  public GetBlocksFunc        getBlocksFunc() { return getBlocksFromTiles16Pal1;}
  public SetBlocksFunc        setBlocksFunc() { return setBlocksFromTiles16Pal1;}
  
  public GetPalFunc           getPalFunc()           { return getPallete;}
  public SetPalFunc           setPalFunc()           { return null;}
  
  //----------------------------------------------------------------------------
  public int getVideoAddress(int id)
  {
    return -1;
  }
  
  public byte[] getVideoChunk(int videoPageId)
  {
     return Utils.readVideoBankFromFile("chr3.bin", videoPageId);
  }
  
  public byte[] getPallete(int palId)
  {
    return Utils.readBinFile("pal3.bin");
  }
  
  public ObjRec vertMirror(ObjRec obj)
  {
     var ind = new int[16];
     
     ind[0] = obj.indexes[12];
     ind[1] = obj.indexes[13];
     ind[2] = obj.indexes[14];
     ind[3] = obj.indexes[15];
     
     ind[4] = obj.indexes[8];
     ind[5] = obj.indexes[9];
     ind[6] = obj.indexes[10];
     ind[7] = obj.indexes[11];
     
     ind[8] = obj.indexes[4];
     ind[9] = obj.indexes[5];
     ind[10] = obj.indexes[6];
     ind[11] = obj.indexes[7];
     
     ind[12] = obj.indexes[0];
     ind[13] = obj.indexes[1];
     ind[14] = obj.indexes[2];
     ind[15] = obj.indexes[2];
     
     return new ObjRec(4, 4, obj.type, ind, obj.palBytes);
  }
  
  public ObjRec[] getBlocksFromTiles16Pal1(int blockIndex)
  {
      int tileAddr = ConfigScript.getTilesAddr(blockIndex);
      var bb = Utils.readBlocksLinearTiles16Pal1(Globals.romdata, tileAddr, ConfigScript.getPalBytesAddr(), ConfigScript.getBlocksCount());
      for (int i = 0; i < bb.Length; i++)
      {
        bb[i] = vertMirror(bb[i]);
      }
      return bb;
  }

  public void setBlocksFromTiles16Pal1(int blockIndex, ObjRec[] blocksData)
  {
    int tileAddr = ConfigScript.getTilesAddr(blockIndex);
    for (int i = 0; i < blocksData.Length; i++)
    {
      blocksData[i] = vertMirror(blocksData[i]); //TODO: remove inplace changes
    }
    Utils.writeBlocksLinearTiles16Pal1(blocksData, Globals.romdata,tileAddr, ConfigScript.getPalBytesAddr(), ConfigScript.getBlocksCount());
  }
  
  public int getBigTileNoFromScreen(int[] screenData, int index)
  {
    int w = getScreenWidth();
    int noY = index / w;
    noY = (noY/8)*8 + 7 - (noY%8);
    int noX = index % w;
    return screenData[noY*w + noX];
  }

  public void setBigTileToScreen(int[] screenData, int index, int value)
  {
    int w = getScreenWidth();
    int noY = index / w;
    noY = (noY/8)*8 + 7 - (noY%8);
    int noX = index % w;
    screenData[noY*w + noX] = value;
  }
}