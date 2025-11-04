using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    private GameObject parentObject; // Das GameObject, das die gesammelten Objekte enthalten soll
    private BoxCollider boxCollider; // Der BoxCollider, der den Bereich definiert
    private Transform[] originalParents; // Array zum Speichern der ursprünglichen Eltern der gesammelten Objekte
    public GameObject flowersParent;
    public GameObject humourousObjectsParent;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        // Sicherstellen, dass ein BoxCollider vorhanden ist
        if (boxCollider == null)
        {
            Debug.LogError("Kein BoxCollider am GameObject gefunden.");
        }
    }

    public GameObject CollectObjects(GameObject mainStatue)
    {
        // Sicherstellen, dass ein BoxCollider vorhanden ist
        if (mainStatue.activeSelf == false)
        {
            parentObject = new GameObject(mainStatue.name);
        }
        else
        {
            parentObject = mainStatue;
        }

        // Suchen Sie alle Collider in der Box
        Collider[] colliders = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents);

        // Speichern der ursprünglichen Eltern der gesammelten Objekte
        originalParents = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            originalParents[i] = colliders[i].transform.parent;
        }

        // Iterieren Sie über alle gefundenen Collider
        foreach (Collider col in colliders)
        {
            if (col.tag == "Interactable" || col.tag == "Paintable")
            {
                if(!col.gameObject.name.EndsWith("Sockel")) //Falls Miniversion in den exporter geworfen wird
                {
                    col.transform.parent = parentObject.transform;
                }
                
            }
        }
        
        return parentObject;
    }

    public void RestoreOriginalParents()
    {
        if (parentObject == null)
        {
            Debug.LogWarning("Es wurden keine Objekte gesammelt.");
            return;
        }

        // Iteriere über alle Kinder des parentObject
        int childCount = parentObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = parentObject.transform.GetChild(i);
            string childName = child.gameObject.name;

            // Prüfe die Endung des Namens und hänge es entsprechend an den richtigen Parent
            if (childName.EndsWith("_flower"))
            {
                child.parent = flowersParent.transform;
            }
            else if (childName.EndsWith("_humourous"))
            {
                child.parent = humourousObjectsParent.transform;
            }
            else
            {
                child.parent = null; // Entferne das Kind aus der aktuellen Hierarchie
            }
        }
    }
}