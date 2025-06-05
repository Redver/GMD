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
        [SerializeField] private Nation selectorNation;


        private void Awake()
        {
            presenter = new SelectorPresenter(this);
        }

        void Start()
        {
            ChangeSelectionParent(startProvince);
            presenter.updateModelSelectedProvinceObject(startProvince); 
            presenter.updateModelCurrentCountryObject(selectorNation.gameObject);
        }
        
        void Update()
        {
            if (isMoving)
            {
                presenter.moveSelector(input);
            }
        }


        public Nation getNation()
        {
            return presenter.getNation();
        }

        public bool canEndTurn()
        {
            return presenter.canEndTurn();
        }

        public bool tryEndTurn()
        {
            if (isActiveAndEnabled)
            {
                return presenter.tryEndTurn();
            }
            playForbiddenSound();
            return false;
        }


        public void dropUnitInNewProvince(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (isActiveAndEnabled && !(buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen()))
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

        public bool canOpenBuildMenuHere()
        {
            return presenter.canOpenBuildMenuHere();
        }


        public void closeBuildMenu()
        {
            if (buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen())
            {
                ToggleBuildMenu();
            }
        }

        public void stopMovement()
        {
            presenter.stopMovement();
            isMoving = false;
        }

        public void inputToggleBuildMenu(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                ToggleBuildMenu();
            }
        }

        public void ToggleBuildMenu()
        {
            if (isActiveAndEnabled && canOpenBuildMenuHere() && presenter.noButtonSelected())
            {
                playButtonSound();
                stopMovement();
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
            else if (presenter.buttonSelected())
            {
                playButtonSound();
                stopMovement();
                presenter.activateButton();
            }
            else
            {
                playForbiddenSound();
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

        public GameObject getButtonBelowCursor(){
            Vector3 rayDirection = new Vector3(0f, -1f, 0f).normalized;
            
            int layerMask = LayerMask.GetMask("Button");
            float yOffset = 0.1f;
            Vector3 adjustedPosition = this.transform.position + Vector3.up * yOffset;
            
            RaycastHit2D hit = Physics2D.Raycast(adjustedPosition, rayDirection, 0.05f,layerMask);
            

            
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }

        public GameObject getProvinceBelowCursor()
        {
            Vector3 rayDirection = new Vector3(0f, -1f, 0f).normalized;
            
            int layerMask = LayerMask.GetMask(nameof(Province), "SeaTile");
            float yOffset = 0.1f;
            Vector3 adjustedPosition = this.transform.position + Vector3.up * yOffset;
            
            RaycastHit2D hit = Physics2D.Raycast(adjustedPosition, rayDirection, 0.05f,layerMask);
            

            
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

            float totalWidth = 0.4f;
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
            
        private void playForbiddenSound()
        {
            SoundLibrary.Instance.PlayClipAtPoint(SoundLibrary.Instance.GetForbiddenSfx(), transform.position);
        }

        private void playButtonSound()
        {
            SoundLibrary.Instance.PlayClipAtPoint(SoundLibrary.Instance.GetRandomButtonClick(), transform.position);
        }
    }

}