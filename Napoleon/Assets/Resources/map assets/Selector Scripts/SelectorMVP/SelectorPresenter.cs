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
          model.SetCoroutineRunner(view);
      }
      
      public void selectUnitInProvince()
      {
          if (model.canSelectUnit())
          {
              model.selectUnitInProvince();
          }
      }
      
      public void deselectUnitInProvince()
      {
          if (model.canDeselectUnit())
          {
              model.delectUnitInProvince();
          }
      }

      public void dropUnitInProvince()
      {
          if (model.canDropUnit())
          {
              model.dropUnitInProvince();
          }
      }

      public void moveSelector(Vector2 input)
      {
          GameObject hitProvinceObject = view.getProvinceBelowCursor();
          if (hitProvinceObject != null)
          {
              processProvinceSelection(hitProvinceObject);
          }
          view.moveSelector(input);
          model.accelerate();
      }

      public void decelerate()
      {
          model.decelerate();
      }

      public void processProvinceSelection(GameObject hitProvinceObject)
      {
          model.updateSelectedProvince(hitProvinceObject);
          view.ChangeSelectionParent(hitProvinceObject);
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
      

      public bool getModelUnitSelected()
      {
          return model.UnitSelected;
      }

      public GameObject getCurrentCountryCapitalProvince()
      {
          return model.CurrentCountryNation.getCurrentCapitalProvince();
      }
  }

}