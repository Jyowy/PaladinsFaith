using FirstSlice.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace FirstSlice
{
    [TrackClipType(typeof(DialogSignalEmitter))]
    public class DialogTrack : SignalTrack
    {
    }
}
