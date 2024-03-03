using UnityEngine;

namespace UnityToolkit
{
    public static class DebugDrawer
    {
        public static void DrawWireCube(Vector3 center, Vector3 size, Color color)
        {
            Vector3 first = center + new Vector3(size.x / 2, size.y / 2, size.z / 2);
            Vector3 second = center + new Vector3(size.x / 2, size.y / 2, -size.z / 2);
            Vector3 third = center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2);
            Vector3 fourth = center + new Vector3(-size.x / 2, size.y / 2, size.z / 2);
            Vector3 fifth = center + new Vector3(size.x / 2, -size.y / 2, size.z / 2);
            Vector3 sixth = center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2);
            Vector3 seventh = center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2);
            Vector3 eighth = center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2);
            
            Debug.DrawLine(first, second, color);
            Debug.DrawLine(second, third, color);
            Debug.DrawLine(third, fourth, color);
            Debug.DrawLine(fourth, first, color);
            
            Debug.DrawLine(fifth, sixth, color);
            Debug.DrawLine(sixth, seventh, color);
            Debug.DrawLine(seventh, eighth, color);
            Debug.DrawLine(eighth, fifth, color);
            
            Debug.DrawLine(first, fifth, color);
            Debug.DrawLine(second, sixth, color);
            
            Debug.DrawLine(third, seventh, color);
            Debug.DrawLine(fourth, eighth, color);
            
            
        }
    }
}