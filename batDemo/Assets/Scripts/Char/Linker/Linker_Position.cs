using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



public class Linker_Position : ILinker
{
    private ObjBase _linker;
    private ObjBase _target;
    private Vector3 _offset;
    public Linker_Position(ObjBase linker, ObjBase target, Vector3 offset)
    {
        _linker = linker;
        _target = target;
        _offset = offset;
    }

    public bool doLink()
    {
        if (_linker == null || _target == null|| _linker.isDead || _target.isDead ) return true;
        _linker.gameObject.transform.position = _target.gameObject.transform.TransformPoint(_offset);
        return false;
    }
    public void dispose()
    {
        _linker = null;
        _target = null;
    }
}
   

