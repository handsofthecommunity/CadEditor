﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CadEditor
{
    public partial class SegaBlockEdit : Form
    {
        public SegaBlockEdit()
        {
            InitializeComponent();
        }

        int curActiveBlock = 0;
        int curActiveTile = 0;
        int curActivePalNo = 0;
        float curScale = 2.0f;
        int curSelectedTilePart = 0;

        ushort[] tiles;
        byte[] videoChunk;
        Color[] cpal;

        const int SEGA_TILES_COUNT = 0x800;

        private void SegaBlockEdit_Load(object sender, EventArgs e)
        {
            reloadTiles();
            Utils.prepareBlocksPanel(pnBlocks, ilSegaTiles.ImageSize, ilSegaTiles, buttonObjClick, 0, SEGA_TILES_COUNT);
            Utils.setCbItemsCount(cbPalSubpart, 4);
            Utils.setCbIndexWithoutUpdateLevel(cbPalSubpart, cbPalNo_SelectedIndexChanged);

            Utils.setCbItemsCount(cbBlockNo, ConfigScript.getBigBlocksCount(), inHex:true);
            Utils.setCbItemsCount(cbTile, SEGA_TILES_COUNT, inHex:true);
            Utils.setCbItemsCount(cbPal, 4);
            Utils.setCbIndexWithoutUpdateLevel(cbBlockNo, cbBlockNo_SelectedIndexChanged);
            resetControls();
        }

        void reloadTiles()
        {
            var mapping = ConfigScript.getBigBlocks(0); //curActiveBigBlock;
            tiles = Mapper.LoadMapping(mapping);
        }

        void saveTiles()
        {
            byte[] tileBytes = new byte[tiles.Length*2];
            Mapper.ApplyMapping(ref tileBytes, tiles);
            ConfigScript.setBigBlocks(0, tileBytes);
            Globals.flushToFile();
        }

        void resetControls()
        {
            fillSegaTiles();
            Utils.reloadBlocksPanel(pnBlocks, ilSegaTiles, 0, SEGA_TILES_COUNT);
            pnBlocks.Invalidate(true);
        }

        private void buttonObjClick(Object button, EventArgs e)
        {
            int index = ((Button)button).ImageIndex;
            pbActive.Image = ilSegaTiles.Images[index];
            curActiveTile = index;
        }

        private void fillSegaTiles()
        {
            videoChunk = ConfigScript.getVideoChunk(0);
            byte[] pal = ConfigScript.getPal(0);
            cpal = VideoSega.GetPalette(pal);
            ilSegaTiles.Images.Clear();
            for (ushort idx = 0; idx < SEGA_TILES_COUNT; idx++)
            {
                ilSegaTiles.Images.Add(VideoSega.GetZoomTile(videoChunk, idx, cpal, (byte)curActivePalNo, false, false, curScale));
            }
        }

        private void cbPalNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPalSubpart.SelectedIndex == -1)
                return;
            curActivePalNo = cbPalSubpart.SelectedIndex;
            resetControls();
        }

        private void cbBlockNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBlockNo.SelectedIndex == -1)
                return;
            curActiveBlock = cbBlockNo.SelectedIndex;
            curSelectedTilePart = 0;
            mapScreen.Invalidate();

            int TILE_WIDTH = 2;
            int TILE_HEIGHT = 2;
            int TILE_SIZE = TILE_WIDTH * TILE_HEIGHT;
            updateMappingControls(curActiveBlock * TILE_SIZE + curSelectedTilePart);
        }

        private void updateMappingControls(int index)
        {
            ushort word = tiles[index];
            Utils.setCbIndexWithoutUpdateLevel(cbTile, cbTile_SelectedIndexChanged, Mapper.TileIdx(word));
            Utils.setCbIndexWithoutUpdateLevel(cbPal, cbPal_SelectedIndexChanged, Mapper.PalIdx(word));
            Utils.setCbCheckedWithoutUpdateLevel(cbHFlip, cbHFlip_CheckedChanged, Mapper.HF(word));
            Utils.setCbCheckedWithoutUpdateLevel(cbVFlip, cbVFlip_CheckedChanged, Mapper.VF(word));
            Utils.setCbCheckedWithoutUpdateLevel(cbPrior, cbPrior_CheckedChanged, Mapper.P(word));
        }

        private void mapScreen_Paint(object sender, PaintEventArgs e)
        {
            int TILE_WIDTH = 2;
            int TILE_HEIGHT = 2;
            int TILE_SIZE = TILE_WIDTH * TILE_HEIGHT;
            int BLOCK_WIDTH = 32;
            int BLOCK_HEIGHT =32;
            int index = curActiveBlock * TILE_SIZE;
            var g = e.Graphics;
            
            for (int i = 0; i < TILE_SIZE; i++)
            {
                ushort word = tiles[index + i];
                var tileRect = new Rectangle(i % TILE_WIDTH * BLOCK_WIDTH, i / TILE_WIDTH * BLOCK_HEIGHT, BLOCK_WIDTH, BLOCK_HEIGHT);
                ushort tileIdx = Mapper.TileIdx(word);
                byte pal = Mapper.PalIdx(word);
                bool hf = Mapper.HF(word);
                bool vf = Mapper.VF(word);
                var b = VideoSega.GetZoomTile(videoChunk, tileIdx, cpal, pal, hf, vf, curScale);
                g.DrawImage(b, tileRect);
                if (i == curSelectedTilePart)
                    g.DrawRectangle(new Pen(Brushes.Red, 2.0f), tileRect);
            }
        }

        private void mapScreen_MouseClick(object sender, MouseEventArgs e)
        {
            int BLOCK_WIDTH = 32;
            int BLOCK_HEIGHT = 32;
            int TILE_WIDTH = 2;
            int TILE_HEIGHT = 2;
            int TILE_SIZE = TILE_WIDTH * TILE_HEIGHT;
            int index = curActiveBlock * TILE_SIZE;

            int dx = e.X / (int)(BLOCK_WIDTH);
            int dy = e.Y / (int)(BLOCK_HEIGHT);
            if (dx < 0 || dx >= TILE_WIDTH || dy < 0 || dy >= TILE_HEIGHT)
                return;
            int tileIdx = dy * TILE_WIDTH + dx;
            curSelectedTilePart = tileIdx;
            int changeIndex = getCurTileIdx();
            //
            if (e.Button == MouseButtons.Left)
            {
                ushort w = tiles[changeIndex];
                w = Mapper.ApplyPalIdx(w, (byte)curActivePalNo);
                w = Mapper.ApplyTileIdx(w, (ushort)curActiveTile);
                tiles[changeIndex] = w;
            }
            else
            {
                curActiveTile = Mapper.TileIdx(tiles[changeIndex]);
                pbActive.Image = ilSegaTiles.Images[curActiveTile];
            }
            //

            updateMappingControls(index + tileIdx);
            mapScreen.Invalidate();
        }

        private int getCurTileIdx()
        {
            int TILE_WIDTH = 2;
            int TILE_HEIGHT = 2;
            int TILE_SIZE = TILE_WIDTH * TILE_HEIGHT;
            return curSelectedTilePart + curActiveBlock * TILE_SIZE;
        }

        private void cbTile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTile.SelectedIndex == -1)
                return;
            int tileIdx = getCurTileIdx();
            ushort val = (ushort)cbTile.SelectedIndex;
            tiles[tileIdx] = Mapper.ApplyTileIdx(tiles[tileIdx], val);
            mapScreen.Invalidate();
        }

        private void cbPal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPal.SelectedIndex == -1)
                return;
            int tileIdx = getCurTileIdx();
            byte val = (byte)cbPal.SelectedIndex;
            tiles[tileIdx] = Mapper.ApplyPalIdx(tiles[tileIdx], val);
            mapScreen.Invalidate();
        }

        private void cbHFlip_CheckedChanged(object sender, EventArgs e)
        {
            int tileIdx = getCurTileIdx();
            int val = cbHFlip.Checked ? 1 : 0;
            tiles[tileIdx] = Mapper.ApplyHF(tiles[tileIdx], val);
            mapScreen.Invalidate();
        }

        private void cbVFlip_CheckedChanged(object sender, EventArgs e)
        {
            int tileIdx = getCurTileIdx();
            int val = cbVFlip.Checked ? 1 : 0;
            tiles[tileIdx] = Mapper.ApplyVF(tiles[tileIdx], val);
            mapScreen.Invalidate();
        }

        private void cbPrior_CheckedChanged(object sender, EventArgs e)
        {
            int tileIdx = getCurTileIdx();
            int val = cbVFlip.Checked ? 1 : 0;
            tiles[tileIdx] = Mapper.ApplyP(tiles[tileIdx], val);
            mapScreen.Invalidate();
        }

        private void tbbSave_Click(object sender, EventArgs e)
        {
            saveTiles();
        }
    }
}
