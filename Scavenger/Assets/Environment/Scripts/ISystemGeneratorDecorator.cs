using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISystemGeneratorDecorator
{
    void System();
    void Stars();
    void PopulateStars();
    void Planets();
    void PopulatePlanets();
    void Moons();
    void PopulateMoons();
    void Dungeons();
    void PopulateDungeons();
}
