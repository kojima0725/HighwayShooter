using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数のトランスフォームを取得可能とする
/// </summary>
public interface ICanGetTransforms
{
    /// <summary>
    /// 複数のトランスフォームを取得する
    /// </summary>
    /// <returns></returns>
    IEnumerable<Transform> Transforms();
}
