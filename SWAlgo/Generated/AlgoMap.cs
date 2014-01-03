/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class AlgoMap : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal AlgoMap(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(AlgoMap obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~AlgoMap() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          SWAlgoPINVOKE.delete_AlgoMap(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public AlgoMap() : this(SWAlgoPINVOKE.new_AlgoMap(), true) {
  }

  public void BuildMap(int size, int rndSeed) {
    SWAlgoPINVOKE.AlgoMap_BuildMap(swigCPtr, size, rndSeed);
  }

  public int GetRandomSeed() {
    int ret = SWAlgoPINVOKE.AlgoMap_GetRandomSeed(swigCPtr);
    return ret;
  }

  public int GetTileType(int x, int y) {
    int ret = SWAlgoPINVOKE.AlgoMap_GetTileType(swigCPtr, x, y);
    return ret;
  }

  public int GetStartTileX(int playerId) {
    int ret = SWAlgoPINVOKE.AlgoMap_GetStartTileX(swigCPtr, playerId);
    return ret;
  }

  public int GetStartTileY(int playerId) {
    int ret = SWAlgoPINVOKE.AlgoMap_GetStartTileY(swigCPtr, playerId);
    return ret;
  }

  public bool CanMoveTo(int x1, int y1, int x2, int y2, int unitFaction1) {
    bool ret = SWAlgoPINVOKE.AlgoMap_CanMoveTo(swigCPtr, x1, y1, x2, y2, unitFaction1);
    return ret;
  }

  public bool CanAttackTo(int x1, int y1, int x2, int y2, int unitFaction1, int unitFaction2) {
    bool ret = SWAlgoPINVOKE.AlgoMap_CanAttackTo(swigCPtr, x1, y1, x2, y2, unitFaction1, unitFaction2);
    return ret;
  }

}
