using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DuskModules {

  /// <summary> Serializable DateTime with a presence in Unity inspector editor </summary>
  [System.Serializable]
  public class DuskDateTime {

    public ushort year;
    public ushort month;
    public ushort day;
    public ushort hour;
    public ushort minute;
    public ushort second;
    public ushort millisecond;

    /// <summary> Creates a new DuskDateTime with the given date </summary>
    public DuskDateTime(DateTime dateTime) {
      SetDate(dateTime);
    }

    /// <summary> Sets the date to the given date </summary>
    public void SetDate(DateTime dateTime) {
      year = (ushort)dateTime.Year;
      month = (ushort)dateTime.Month;
      day = (ushort)dateTime.Day;
      hour = (ushort)dateTime.Hour;
      minute = (ushort)dateTime.Minute;
      second = (ushort)dateTime.Second;
      millisecond = (ushort)dateTime.Millisecond;
    }

    /// <summary> Sets the date to now </summary>
    public void SetNow() {
      DateTime dateTime = DateTime.Now;
      SetDate(dateTime);
    }

    /// <summary> Returns the date as a DateTime </summary>
    public DateTime ToDateTime() {
      return new DateTime(year, month, day, hour, minute, second, millisecond);
    }
  }
}