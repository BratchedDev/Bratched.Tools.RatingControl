# Bratched.Tools.RatingControl

![](http://dev.bratched.com/fr/wp-content/uploads/sites/2/2015/03/RateControlIcon_100x100.png)

a complete xaml rating control

More informations in
* [http://dev.bratched.com/fr](http://dev.bratched.com/fr)
* [http://www.bratched.com](http://www.bratched.com)


## Introduction ##

The Bratched Rating Control is used to show or Edit rate information.

This solution include :
 
 * common RatingControl source code
 * Project to build Rating control in Universal App, WP8.1, WP8.0, WP7.0
 * Demo source in Universal App, WP8.1, WP8.0, WP7.0

Compatible with:

 * Windows Phone 7.1
 * Windows Phone 8
 * Windows Phone 8.1
 * Windows 8.1 (WINRT App)


## How to use it ? ##

###Nuget
There is a [nuget package](https://www.nuget.org/packages/BratchedTools.RatingControl/) to easily add RatingControl in your project.

![](http://dev.bratched.com/fr/wp-content/uploads/sites/2/2015/03/NugetRatingControl.png)

In your solution, choose manage nuget package and install Bratched Rating Control. 

###Into the XAML Header Page of your project
Into the Page header add :

####Windows Phone 7
	xmlns:rating="clr-namespace:Bratched.Tools.RatingControl.Controls;assembly=Bratched.Tools.RatingControl_wp71"

####Windows Phone 8.0

	xmlns:rating="clr-namespace:Bratched.Tools.RatingControl.Controls;assembly=Bratched.Tools.RatingControl_wp80"

####Windows Phone 8.1

    xmlns:rating="clr-namespace:Bratched.Tools.RatingControl.Controls;assembly=Bratched.Tools.RatingControl_wp81"

####Universal Apps, Windows 8 and Windows Phone 8.1 XAML app

    xmlns:rating="clr-namespace:Bratched.Tools.RatingControl"

###Into the XAML Page

Add the rate control with this code

    <rating:RatingControl ItemsCount="5" Value="2.5" />

![](http://dev.bratched.com/fr/wp-content/uploads/sites/2/2015/03/VisualStudioRatingControl.png)

##Main Properties of the RatingControl

###ItemsCount
To change the number of rate items

###Value
The value of the rate

###IsEditable
* False : Not Editable
* True : User can change the value with click or tap

###ItemsSpacing
Space between each items in percent. 
Each items has a theoretical width of 100 units. To have the same space that an item you need to have 100 in this value.

###ItemTemplate
Choose predefined forms to change star rate to other pattern like : 

* Heart
* Like
* Smiley
* Trophy
* Star

note: You can also define your own pattern
*(see in advanced properties)*
    
###RoundValueSlice

Round the Editing value.

**example 1 : RoundValueSlice = 0.5** 
will accept only values like 0.5, 1, 1.5, 2, 2.5, ...

**example 2 : RoundValueSlice = 0.25** 
will accept only values like 0.25, 0.5, 0.75, 1, 1.25, 1.5, ...   

if RoundValueSlice == 0,all the values are accepted.

## Advanced Properties of the RatingControl
### Principe
You can personalize aspect of ratingcontrols with 2 lists of properties :

* EmptyItemsDefinition
* FullItemsDefinition

To change background color of the empty rate items you can use this syntax (Change the background color of the default yellow star in Red):

    <rating:RatingControl ItemsCount="5" Value="1" >
   		<rating:RatingControl.EmptyItemsDefinition>
    		<rating:RateItemDefinition BackgroundColor="Red"/>
    	</rating:RatingControl.EmptyItemsDefinition>
    </rating:RatingControl>

To Change the background color of the full rate items you can use this syntax.

    <rating:RatingControl ItemsCount="5" Value="1" >
   		<rating:RatingControl.FullItemsDefinition>
    		<rating:RateItemDefinition BackgroundColor="Red"/>
    	</rating:RatingControl.FullItemsDefinition>
    </rating:RatingControl>

Of course you can combine empty and full items but you can also define cyclic schemes like this.

*(first empty items are in green, last are in yellow and all the selected items are red)*

     <rating:RatingControl ItemsCount="5" Value="1" >
    	<rating:RatingControl.EmptyItemsDefinition>
    		<rating:RateItemDefinition BackgroundColor="Green"/>
    		<rating:RateItemDefinition BackgroundColor="Yellow"/>
    	</rating:RatingControl.EmptyItemsDefinition>
    	<rating:RatingControl.FullItemsDefinition>
    		<rating:RateItemDefinition BackgroundColor="Red"/>
    	</rating:RatingControl.FullItemsDefinition>
    </rating:RatingControl>

### RateItemsDefinition properties

#### BackgroundColor
The color of the item

#### OutlineColor
The color of the outline of the figure

#### OutlineThikness
The depth of the outline of the figure

#### PathData
The geometry Path Data of the figure in a string.	
See [https://msdn.microsoft.com/fr-fr/library/ms752293(v=vs.110).aspx](https://msdn.microsoft.com/fr-fr/library/ms752293(v=vs.110).aspx)

Example with all the properties

     <rating:RatingControl ItemsCount="8" Value="2">
        <rating:RatingControl.EmptyItemsDefinition>
            <rating:RateItemDefinition 
                BackgroundColor="Cyan"
                OutlineColor="Blue"
                OutlineThikness="4"
                PathData="M 0,0 A 15,5 180 1 1 200,0 L 200,100 L 300,100 
                    L 300,200 A 15,5 180 1 1 100,200 L 100,100 L 0,100 Z"/>
        </rating:RatingControl.EmptyItemsDefinition>
        <rating:RatingControl.FullItemsDefinition>
            <rating:RateItemDefinition
                BackgroundColor="Blue"
                OutlineColor="Red"
                OutlineThikness="8"
                PathData="M 0,0 A 15,5 180 1 1 200,0 L 200,100 L 300,100 
                    L 300,200 A 15,5 180 1 1 100,200 L 100,100 L 0,100 Z"/>
        </rating:RatingControl.FullItemsDefinition>
      </rating:RatingControl>
 
 ![](http://dev.bratched.com/fr/wp-content/uploads/sites/2/2015/03/VisualStudioRatingControl2.png)
 
##Version history

 * v1.0 

	* First public release of the current implementation.




