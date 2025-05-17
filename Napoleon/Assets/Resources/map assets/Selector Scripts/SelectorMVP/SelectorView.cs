using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using IUnit = Resources.Features.Model.Units.IUnit;

namespace Resources.map_assets.Selector_Scripts.SelectorMVP
{
    public class SelectorView : MonoBehaviour
    {
        [SerializeField] private GameObject startProvince;
        [SerializeField] private Vector2 input;
        private bool isMoving = false;
        [SerializeField] private GameObject buildMenu;
        private SelectorPresenter presenter;

        private void Awake()
        {
            presenter = new SelectorPresenter(this);
        }

        void Start()
        {
            ChangeSelectionParent(startProvince);
            presenter.updateModelSelectedProvinceObject(startProvince); 
            presenter.updateModelCurrentCountryObject(startProvince.GetComponent<Province>().getOwnerGameObject());
        }
        
        void Update()
        {
            if (isMoving)
            {
                presenter.moveSelector(input);
            }
        }

        public void dropUnitInNewProvince(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (isActiveAndEnabled)
                {
                    presenter.dropUnitInProvince();
                }
            }
        }

        public void selectUnitInProvince(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (isActiveAndEnabled)
                {
                    presenter.selectUnitInProvince();
                }
            }
        }

        public void deselectUnitInProvince(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (isActiveAndEnabled)
                {
                    presenter.deselectUnitInProvince();
                }
            }
        }


        public void ToggleBuildMenu()
        {
            if (isActiveAndEnabled)
            {
                if (buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen())
                {
                    buildMenu.GetComponent<BuilderMenuUI>().setClose();
                    buildMenu.SetActive(false);
                }
                else
                {
                    buildMenu.SetActive(true);
                    Vector3 menuPosition = this.transform.position;
                    menuPosition.x += 0.8f;
                    menuPosition.y += 0.3f;
                    buildMenu.transform.position = menuPosition;
                    buildMenu.GetComponent<BuilderMenuUI>().setOpen();
                    buildMenu.GetComponent<BuilderMenuUI>().setProvinceOpenOn(presenter.getSelectedProvinceObject());
                }
            }
        }

        public GameObject getSelectedProvinceObject()
        {
            return presenter.getSelectedProvinceObject();
        }

        public void OnInput(InputAction.CallbackContext context)
        {
            if (!isActiveAndEnabled || buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen()) return;

            if (context.performed)
            {
                input = context.ReadValue<Vector2>();
                isMoving = true;
            }
            else if (context.canceled)
            {
                input = Vector2.zero;
                isMoving = false;
                presenter.decelerate();
            }
        }

        public GameObject getProvinceBelowCursor()
        {
            Vector3 rayDirection = new Vector3(0, -1, 0f).normalized;
            
            int layerMask = LayerMask.GetMask("Province", "SeaTile");
            
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, rayDirection, 0.1f,layerMask);
            

            
            if (hit.collider != null && hit.collider.gameObject != this.getSelectedProvinceObject())
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }
        
        public void resetPosition()
        {
            this.transform.position = this.startProvince.transform.position;
        }
        
        public void ChangeSelectionParent(GameObject province)
        {
                gameObject.transform.SetParent(province.transform); 
        }

        public void moveSelector(Vector2 input)
        {
            float speed = presenter.getSpeed();
            transform.position += speed * Time.deltaTime * new Vector3(input.x, input.y, 0f);
        }

        public void onStartTurn(GameObject nation)
        {
            startOnCapital();
        }

        public void startOnCapital()
        {
            resetPosition();
            presenter.processProvinceSelection(startProvince);
        }
        
        
        public void spreadUnits() //minor changes to work for selector
        {
            int unitCount = presenter.getNumberOfUnits();
            if (unitCount == 0) return;

            Collider2D col = GetComponent<Collider2D>();
            if (col == null) return;

            Bounds bounds = col.bounds;
            float totalWidth = bounds.size.x * 0.4f;
            float spacing = (unitCount > 1) ? totalWidth / (unitCount - 1) : 0;

            float startX = -totalWidth / 2f;
            float yOffset = 0.2f; 

            IUnit[] units = presenter.getAllUnits();

            for (int i = 0; i < unitCount; i++)
            {
                GameObject unitObj = units[i].getView().gameObject;
                unitObj.transform.SetParent(this.transform);

                float x = startX + i * spacing;
                unitObj.transform.localPosition = new Vector3(x, yOffset, 0f);
            }
        }
    }
}