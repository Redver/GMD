using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.map_assets.Selector_Scripts.SelectorMVP
{
    public class SelectorView : MonoBehaviour
    {
        [SerializeField] private GameObject startProvince;
        [SerializeField] private Vector2 input;
        [SerializeField] private GameObject buildMenu;
        private SelectorPresenter presenter;

        private void Awake()
        {
            presenter = new SelectorPresenter(this);
        }

        void Start()
        {
            ChangeSelectionParent(startProvince);
            presenter.startCooldownCoroutine();
            presenter.updateModelSelectedProvinceObject(startProvince); 
            presenter.updateModelCurrentCountryObject(startProvince.GetComponent<Province>().getOwnerGameObject());
        }

        public void startCooldownCoroutine()
        {
            StartCoroutine(startCooldown());
        }

        public void dropUnitInNewProvince()
        {
            if (isActiveAndEnabled)
            {
                presenter.dropUnitInProvince();
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
            if (this.isActiveAndEnabled && !buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen())
            {
                if (presenter.canMove())
                {
                    this.input = context.ReadValue<Vector2>();
                    presenter.moveSelector(input); 
                }
            }
        }

        public GameObject getHitProvinceObject(Vector2 input)
        {
            Vector3 rayDirection = new Vector3(input.x, input.y, 0f).normalized;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, 100f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != this.getSelectedProvinceObject())
                {
                    return hit.collider.gameObject;
                }
            }
            return null; 
        } 
        
        private IEnumerator startCooldown()
        {
            while (presenter.getModelInputCooldown() >= 0f) 
            {
                presenter.updateCooldown(Time.deltaTime); 
                yield return null; 
            }
        }
        
        public void ChangeSelectionParent(GameObject province)
        {
                gameObject.transform.SetParent(province.transform); 
                Vector3 oldPosition = gameObject.transform.position;
                Vector3 newPosition = gameObject.transform.parent.position; 
                StartCoroutine(LerpSelector(oldPosition, newPosition, presenter.getModelMovementTime())); 
        }
        
        private IEnumerator LerpSelector(Vector3 from, Vector3 to, float timeForMovement)
        {
            float elapsedTime = 0f;
            while (elapsedTime < timeForMovement)
            {
                gameObject.transform.position = Vector3.Lerp(from, to, elapsedTime / timeForMovement);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.transform.position = to;
        }
        

        public void onStartTurn(GameObject nation)
        {
            startOnCapital();
            presenter.startCooldownCoroutine();
        }

        public void startOnCapital()
        {
            ChangeSelectionParent(presenter.getCurrentCountryCapitalProvince());
        }
    }
}