using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeTerrainChunk : MonoBehaviour
{
    public float chunkSize = 256;
    QuadtreeNode originNode;
    public TerrainMeshData terrainMeshData;
    public Vector3 center;
    //The vertex error used for LOD
    public static float vertexError;
    void Awake()
    {
        originNode = new QuadtreeNode(null, new QuadTreeVert(center), QuadtreeNode.NodeType.Origin, chunkSize);
    }

    public class QuadtreeNode
    {
        public enum NodeType { Origin, TopLeft, TopRight, BottomLeft, BottomRight }
        //The type of node this is. Used to generate vertices from the parent.
        public NodeType nodeType;

        bool nodeEnabled = false;
        //The parent of this node.
        QuadtreeNode parent = null;
        //The vertices of this node.
        QuadTreeVert left, right, top, bottom, topLeft, topRight, bottomLeft, bottomRight, center;

        //The neighbours of this node.
        public QuadtreeNode neighbourTop, neighbourBottom, neighbourLeft, neighbourRight;
        //The children of this node.
        public QuadtreeNode childTopLeft, childTopRight, childBottomLeft, childBottomRight;

        bool hasChildren = false;

        bool finishedFlag = false;

        //The size of the node. This decreases with every subdivision, used for placing verts.
        float size;

        public QuadtreeNode(QuadtreeNode Parent, QuadTreeVert Center, NodeType NodeType, float Size)
        {
            parent = Parent;
            size = Size;
            center = Center;
            center.y = Noise.SampleNoiseTemp(center.ToVector());
            center.enabled = true;
            nodeType = NodeType;

            //Remove the dependancy on the below logic
            if (size > EndlessTerrain.terrainGenerator.MinQuadtreeSize)
            {
                Subdivide();
            }
            if (parent == null)
            {
                EnableNode(this);
            }
        }

        //Enables the node
        static void EnableNode(QuadtreeNode node)
        {
            node.nodeEnabled = true;
            node.AddVertices();
            node.topLeft.enabled = true;
            node.topRight.enabled = true;
            node.bottomLeft.enabled = true;
            node.bottomRight.enabled = true;
            if (node.parent != null)
            {
                switch (node.nodeType)
                {
                    case NodeType.TopLeft:
                        node.parent.top.enabled = true;
                        node.parent.left.enabled = true;
                        break;
                    case NodeType.TopRight:
                        node.parent.top.enabled = true;
                        node.parent.right.enabled = true;
                        break;
                    case NodeType.BottomRight:
                        node.parent.bottom.enabled = true;
                        node.parent.right.enabled = true;
                        break;
                    case NodeType.BottomLeft:
                        node.parent.bottom.enabled = true;
                        node.parent.left.enabled = true;
                        break;
                }
            }
        }

        void Subdivide()
        {
            hasChildren = true;
            QuadTreeVert topLeftCenter = new QuadTreeVert(center.x - size * .25f, 0, center.z + size * .25f);
            childTopLeft = new QuadtreeNode(this, topLeftCenter, NodeType.TopLeft, size * .5f);

            QuadTreeVert topRightCenter = new QuadTreeVert(center.x + size * .25f, 0, center.z + size * .25f);
            childTopRight = new QuadtreeNode(this, topRightCenter, NodeType.TopRight, size * .5f);

            QuadTreeVert bottomRightCenter = new QuadTreeVert(center.x + size * .25f, 0, center.z - size * .25f);
            childBottomRight = new QuadtreeNode(this, bottomRightCenter, NodeType.BottomRight, size * .5f);

            QuadTreeVert bottomLeftCenter = new QuadTreeVert(center.x - size * .25f, 0, center.z - size * .25f);
            childBottomLeft = new QuadtreeNode(this, bottomLeftCenter, NodeType.BottomLeft, size * .5f);
        }

        //Tests the edge. If the edge's y is within an error of the average of the corners, don't subdivide.
        bool TestEdge(Vector3 cornerA, Vector3 cornerB, Vector3 edge)
        {
            float avgHeight = (cornerA.y + cornerB.y) * .5f;

            if (edge.y >= avgHeight - vertexError && edge.y <= avgHeight + vertexError)
            {
                return false;
            }
            return true;
        }
        //Tests all edges. @todo there is a lot of room for improvement here. right now the entire node is subdivided if any edge fails.
        // It should probably only subdivide the nodes that have edges that fail.
        bool ShouldSubdivide()
        {
            if (!TestEdge(topLeft.ToVector(), topRight.ToVector(), top.ToVector()) && !TestEdge(topRight.ToVector(),
                bottomRight.ToVector(), right.ToVector())
                && !TestEdge(bottomRight.ToVector(), bottomLeft.ToVector(), bottom.ToVector()) &&
                !TestEdge(bottomLeft.ToVector(), topLeft.ToVector(), left.ToVector()))
            {
                finishedFlag = true;
                return false;
            }
            return true;
        }

        //Creates the vertices for this node. Uses as many parent vertices as possible.
        void AddVertices()
        {
            switch (nodeType)
            {
                case NodeType.TopLeft:
                    topLeft = parent.topLeft;
                    topRight = parent.top;
                    bottomLeft = parent.left;
                    bottomRight = parent.center;
                    break;
                case NodeType.TopRight:
                    topLeft = parent.top;
                    topRight = parent.topRight;
                    bottomLeft = parent.center;
                    bottomRight = parent.right;
                    break;

                case NodeType.BottomLeft:
                    topLeft = parent.left;
                    topRight = parent.center;
                    bottomLeft = parent.bottomLeft;
                    bottomRight = parent.bottom;
                    break;

                case NodeType.BottomRight:
                    topLeft = parent.center;
                    topRight = parent.right;
                    bottomLeft = parent.bottom;
                    bottomRight = parent.bottomRight;
                    break;

                default:

                    topLeft = new QuadTreeVert(center.ToVector() + new Vector3(-size * .5f, 0, size * .5f));
                    topLeft.y = Noise.SampleNoiseTemp(topLeft.ToVector());
                    topLeft.enabled = true;

                    topRight = new QuadTreeVert(center.ToVector() + new Vector3(size * .5f, 0, size * .5f));
                    topRight.y = Noise.SampleNoiseTemp(topRight.ToVector());
                    topRight.enabled = true;

                    bottomLeft = new QuadTreeVert(center.ToVector() + new Vector3(-size * .5f, 0, -size * .5f));
                    bottomLeft.y = Noise.SampleNoiseTemp(bottomLeft.ToVector());
                    bottomLeft.enabled = true;

                    bottomRight = new QuadTreeVert(center.ToVector() + new Vector3(size * .5f, 0, -size * .5f));
                    bottomRight.y = Noise.SampleNoiseTemp(bottomRight.ToVector());
                    bottomRight.enabled = true;

                    break;
            }

            top = new QuadTreeVert(center.ToVector() + new Vector3(0, 0, size * .5f));
            top.y = Noise.SampleNoiseTemp(top.ToVector());

            left = new QuadTreeVert(center.ToVector() + new Vector3(-size * .5f, 0, 0));
            left.y = Noise.SampleNoiseTemp(left.ToVector());

            right = new QuadTreeVert(center.ToVector() + new Vector3(size * .5f, 0, 0));
            right.y = Noise.SampleNoiseTemp(right.ToVector());

            bottom = new QuadTreeVert(center.ToVector() + new Vector3(0, 0, -size * .5f));
            bottom.y = Noise.SampleNoiseTemp(bottom.ToVector());

        }

        public void ActivateChildren()
        {
            if (this.hasChildren == false || this.nodeEnabled == false)
            {
                if (nodeEnabled == true)
                {
                    //if(ShouldSubdivide() == false)
                    //{
                    //    center.enabled = false;
                    //}
                    finishedFlag = true;
                }
                return;
            }

            if (ShouldSubdivide())
            {
                EnableNode(childBottomLeft);
                EnableNode(childBottomRight);
                EnableNode(childTopLeft);
                EnableNode(childTopRight);

                top.enabled = true;
                left.enabled = true;
                right.enabled = true;
                bottomRight.enabled = true;

                childTopLeft.ActivateChildren();
                childTopRight.ActivateChildren();
                childBottomLeft.ActivateChildren();
                childBottomRight.ActivateChildren();
            }
        }

        void ForceSubdivide()
        {
            finishedFlag = false;
            EnableNode(childBottomLeft);
            childBottomLeft.finishedFlag = true;
            EnableNode(childBottomRight);
            childBottomRight.finishedFlag = true;
            EnableNode(childTopLeft);
            childTopLeft.finishedFlag = true;
            EnableNode(childTopRight);
            childTopRight.finishedFlag = true;

            top.enabled = true;
            left.enabled = true;
            right.enabled = true;
            bottomRight.enabled = true;
        }
    }


}