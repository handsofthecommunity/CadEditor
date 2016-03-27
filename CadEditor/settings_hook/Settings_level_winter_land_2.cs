using CadEditor;
using System;
using System.Drawing;

public class Data 
{ 
  public OffsetRec getScreensOffset()  { return new OffsetRec( 0x159aa, 1 , 32*24);   }
  public int getScreenWidth()          { return 32; }
  public int getScreenHeight()         { return 24; }
  public string getBlocksFilename()    { return "hook_4.png"; }
  
  public bool isBigBlockEditorEnabled() { return false; }
  public bool isBlockEditorEnabled()    { return false; }
  public bool isEnemyEditorEnabled()    { return false; }
}