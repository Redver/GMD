using UnityEngine;
using IUnit = Resources.Features.Model.Units.IUnit;

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
          else
          {
              playForbiddenSound();
          }
      }
      
      public bool canOpenBuildMenuHere()
      {
          return model.canOpenBuildMenuHere();
      }

      public Nation getNation()
      {
          return model.getNation();
      }

      public void deselectUnitInProvince()
      {
          if (model.canDeselectUnit())
          {
              model.delectUnitInProvince();
          }
          else
          {
              playForbiddenSound();
          }
      }

      public void dropUnitInProvince()
      {
          if (model.canDropUnit())
          {
              model.dropUnitInProvince();
          }
          else
          {
              playForbiddenSound();
          }
      }

      public void moveSelector(Vector2 input)
      {
          GameObject hitProvinceObject = view.getProvinceBelowCursor();
          GameObject hitButtonObject = view.getButtonBelowCursor();
          if (hitProvinceObject != null)
          {
              processProvinceSelection(hitProvinceObject);
          }

          if (hitButtonObject != null)
          {
              model.updateSelectedButton(hitButtonObject);
          }
          else
          {
              model.clearButton();
          }
          view.moveSelector(input);
          model.accelerate();
      }

      public bool noButtonSelected()
      {
          if (model.isButtonSelected())
          {
              return false;
          }
          else
          {
              return true;
          }
      }
      
      public bool buttonSelected()
      {
          if (model.isButtonSelected())
          {
              return true;
          }
          else
          {
              return false;
          }
      }

      public void activateButton()
      {
          model.activateButton();
      }

      public void decelerate()
      {
          model.decelerate();
      }

      public bool canEndTurn()
      {
          return model.canEndTurn();
      }

      public bool tryEndTurn()
      {
          if (model.canEndTurn())
          {
              return model.tryEndTurn();
          }

          playForbiddenSound();
          return false;
      }

      public void stopMovement()
      {
          model.stopMovement();
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

      public float getSpeed()
      {
          return model.Speed;
      }

      public GameObject getCurrentCountryCapitalProvince()
      {
          return model.CurrentCountryNation.getCurrentCapitalProvince();
      }

      public int getNumberOfUnits()
      {
          return model.getNumberOfUnits();
      }

      public IUnit[] getAllUnits()
      {
          return model.getAllUnits();
      }

      private void playForbiddenSound()
      {
          SoundLibrary.Instance.PlayClipAtPoint(SoundLibrary.Instance.GetForbiddenSfx(), view.transform.position);
      }
  }

}