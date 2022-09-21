# Diagram Description

## This will provide some guidance on how to interpret our design diagrams.

The following symbols are used:
- Oval: represents the user
- Rectangles: different components which represent some form of data used by our project
- Arrows: illustrates how one component interacts with another, an arrow pointing from one rectangle to another means the source rectangle is generating or outputting some event or data that is input into the target rectangle

## The User
The user is our main source of input. They will control our game via a mouse and keyboard and receive output from the game in the form of a visual response from the graphical layer.

## UI Layer
The UI layer is responsible for helping the user interact with the game in the form of displaying text and effects on the screen.

## Graphical Layer
The graphical layer handles the rendering of 3D or 2D objects/sprites and passes information on to the event layer to manipulate the objects which it renders.

## Event Layer
The event layer is the "brains" of our games and is responsible for making complex calculations, controlling the objects rendered on the graphical layer, making decisions based on user input or game settings, amongst other event-based actions.

## Data Layer
The data layer holds settings manipulated in the UI layer and saves the state of the objects managed by the event layer.
