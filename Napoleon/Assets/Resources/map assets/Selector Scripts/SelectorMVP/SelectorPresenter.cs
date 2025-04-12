using System.Collections;
using System.Collections.Generic;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.map_assets.Selector_Scripts.SelectorMVP
{
  public class SelectorPresenter
  {
      private SelectorModel model;
      private SelectorView view;
      public SelectorPresenter(SelectorView selectorView)
      {
          view = selectorView;
          model = new SelectorModel(this);
      }

      public bool canMove()
      {
          return model.InputCooldown <= 0f;
      }
      
      public void moveSelector(Vector2 input)
      {
          GameObject hitProvinceObject = view.getHitProvinceObject(input);
          if (hitProvinceObject != null)
          {
              processProvinceSelection(hitProvinceObject);
          }
      }
      
      public void processProvinceSelection(GameObject hitProvinceObject)
      {
          model.SelectedProvinceObject = hitProvinceObject;
          view.ChangeSelectionParent(hitProvinceObject);
          resetCooldown();
          startCooldownCoroutine();

      }

      public bool canSelectProvince(GameObject hitProvinceObject)
      {
          return model.canSelectProvince(hitProvinceObject);
      }

      public void startCooldownCoroutine()
      {
          view.startCooldownCoroutine();
      }

      public void updateModelSelectedProvinceObject(GameObject selectedProvince)
      {
          model.SelectedProvinceObject = selectedProvince;
          model.SelectedProvince = selectedProvince.GetComponent<Province>();
      }

      public GameObject getSelectedProvinceObject()
      {
          return model.SelectedProvinceObject;
      }
      
      public void updateModelCurrentCountryObject(GameObject currentCountry)
      {
          model.CurrentCountryObject = currentCountry;
          model.CurrentCountryNation = currentCountry.GetComponent<Nation>();
      }

      public GameObject getCurrentCountryObject()
      {
          return model.CurrentCountryObject;
      }

      public float getModelInputCooldown()
      {
          return model.InputCooldown;
      }

      public float getModelMovementTime()
      {
          return model.MovementTime;
      }

      public bool getModelUnitSelected()
      {
          return model.UnitSelected;
      }

      public void updateCooldown(float deltaTime)
      {
          model.InputCooldown -= deltaTime;
      }

      public void resetCooldown()
      {
          model.InputCooldown = model.CooldownBaseValue;
      }

      public GameObject getCurrentCountryCapitalProvince()
      {
          return model.CurrentCountryNation.getCurrentCapitalProvince();
      }
  }

}