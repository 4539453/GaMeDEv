#include "ferox.h"
#include "raylib.h"
#include "raymath.h"

#define TARGET_FPS 60

#define SCREEN_WIDTH 800
#define SCREEN_HEIGHT 600

#define SCREEN_WIDTH_IN_METERS (SCREEN_WIDTH / FR_GLOBAL_PIXELS_PER_METER)
#define SCREEN_HEIGHT_IN_METERS (SCREEN_HEIGHT / FR_GLOBAL_PIXELS_PER_METER)

#define WORLD_RECTANGLE ((Rectangle) { 0, 0, SCREEN_WIDTH_IN_METERS, SCREEN_HEIGHT_IN_METERS })

#define BODY_MATERIAL ((frMaterial) { 20.0f, 0.0f,  100.0f, 100.0f })
// #define BODY_MATERIAL ((frMaterial) { 100.0f, 0.0f,  0.0f, 0.0f })
#define BOUNDARY_MATERIAL FR_DYNAMICS_DEFAULT_MATERIAL

static frWorld *InitGame(void);
static void UpdateGame(frWorld *);
static void DrawGame(frWorld *);
static frBody *CreateBoundaryDown(Color);
static frBody *CreateBoundaryUp(Color);
static frBody *CreateBoundaryLeft(Color);
static frBody *CreateBoundaryRight(Color);
static frBody *CreateColoredCircle(Color);
static frShape *GetTargetShape(frWorld *, Vector2);

int main(void) {
  SetConfigFlags(FLAG_MSAA_4X_HINT);
  SetTargetFPS(TARGET_FPS);
  
  InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Bouncing Balls");

  frWorld *world = InitGame();

  while (!WindowShouldClose()) {
    UpdateGame(world);
    DrawGame(world);
  }
  
  frReleaseWorldBodies(world);
  CloseWindow();
  return 0;
}


frWorld *InitGame(){
  // Create world
  frWorld *world = frCreateWorld(
    frVec2ScalarMultiply(FR_WORLD_DEFAULT_GRAVITY, 0.00001f),
    WORLD_RECTANGLE
  );

  // Create boundaries
  frBody *boundaryDown = CreateBoundaryDown(GRAY);
  frBody *boundaryUp = CreateBoundaryUp(GRAY);
  frBody *boundaryRight = CreateBoundaryLeft(GRAY);
  frBody *boundaryLeft = CreateBoundaryRight(GRAY);
  
  // add boundaries to world
  frAddToWorld(world, boundaryUp);
  frAddToWorld(world, boundaryDown);
  frAddToWorld(world, boundaryRight);
  frAddToWorld(world, boundaryLeft);

  return world;
}


void UpdateGame(frWorld *world){

  if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON)) {
    frShape *targetShape = GetTargetShape(world, GetMousePosition());
    bool collisionTrue = false;
    if (targetShape != NULL) collisionTrue = true;

    if(!collisionTrue)
    {
      frBody *circle = CreateColoredCircle(frGetRandomColor());
      frAddToWorld(world, circle);
    }
    else if(frGetShapeType(targetShape) == FR_SHAPE_CIRCLE &&
            ColorToInt(frGetShapeColor(targetShape)) != ColorToInt(WHITE))
    {
      frSetShapeColor(targetShape, WHITE);
    }
    else if(frGetShapeType(targetShape) == FR_SHAPE_CIRCLE &&
            ColorToInt(frGetShapeColor(targetShape)) == ColorToInt(WHITE))
    {
      frSetShapeColor(targetShape, frGetShapeInitColor(targetShape));
    }
  }


  if (IsMouseButtonDown(MOUSE_BUTTON_RIGHT)) {
      for (int i = 0; i < frGetWorldBodyCount(world); i++) {
        frBody *body = frGetWorldBody(world, i);
        frShape *shape = frGetBodyShape(body);
      if(frGetShapeType(shape) == FR_SHAPE_CIRCLE &&
          ColorToInt(frGetShapeColor(shape)) == ColorToInt(WHITE))
      {
        float distance = Vector2Distance(
                            frVec2PixelsToMeters(GetMousePosition()),
                            frGetBodyPosition(body)
                                          );

        if (distance <= 5.0f) 
          frSetShapeColor(shape, frGetShapeInitColor(shape));

        float ds = 1.5f / distance;
        Vector2 targetVec = Vector2Lerp(
                                frGetBodyPosition(body),
                                frVec2PixelsToMeters(GetMousePosition()),
                                ds
                                          );
        frSetBodyPosition(body, targetVec);
      }
    }
  }

  // remove bodies out of screen
  for (int i = 0; i < frGetWorldBodyCount(world); i++) {
    frBody *body = frGetWorldBody(world, i);
    
    if (!CheckCollisionRecs(frGetBodyAABB(body), WORLD_RECTANGLE))
      frRemoveFromWorld(world, body);
  }
}

void DrawGame(frWorld *world){
  BeginDrawing();

  ClearBackground(LIGHTGRAY);
  
  for (int i = 0; i < frGetWorldBodyCount(world); i++){
    if (frGetBodyType)
      frDrawBody(frGetWorldBody(world, i));
  }
  
  frDrawSpatialHash(frGetWorldSpatialHash(world));

  frSimulateWorld(world, (1.0f / 60.0f) * 100);
  
  DrawTextEx(
    GetFontDefault(),
    TextFormat(
      "bodies.count: %d\n",
      frGetWorldBodyCount(world) - 4
    ),
    (Vector2) { 32, 32 },
    20,
    1, 
    WHITE
  );

  EndDrawing();
}

frBody *CreateBoundaryDown(Color color){
  frBody *body = frCreateBodyFromShape(
        FR_BODY_KINEMATIC, 
        (Vector2) { SCREEN_WIDTH_IN_METERS * 0.5f, SCREEN_HEIGHT_IN_METERS},
        frCreateRectangle(
          BOUNDARY_MATERIAL,
          (Vector2) {SCREEN_WIDTH_IN_METERS, 2.0f },
          (Vector2) { 0, 0 },
          color
        )
  );
  return body;
}
  
frBody *CreateBoundaryUp(Color color){
  frBody *body = frCreateBodyFromShape(
         FR_BODY_KINEMATIC, 
        (Vector2) { SCREEN_WIDTH_IN_METERS * 0.5f, 0},
        frCreateRectangle(
          BOUNDARY_MATERIAL,
          (Vector2) { SCREEN_WIDTH_IN_METERS , 2.0f },
          (Vector2) { 0, 0 },
          color
        )
  );
  return body;
}
  
frBody *CreateBoundaryLeft(Color color){
  frBody *body = frCreateBodyFromShape(
        FR_BODY_KINEMATIC, 
        (Vector2) { SCREEN_WIDTH_IN_METERS, SCREEN_HEIGHT_IN_METERS * 0.5f},
        frCreateRectangle(
          BOUNDARY_MATERIAL,
          (Vector2) {2.0f, SCREEN_HEIGHT_IN_METERS  - 2.0f},
          (Vector2) { 0, 0 },
          color
        )
  );
  return body;
}
  
frBody *CreateBoundaryRight(Color color){
  frBody *body = frCreateBodyFromShape(
        FR_BODY_KINEMATIC, 
        (Vector2) { 0, SCREEN_HEIGHT_IN_METERS * 0.5f},
        frCreateRectangle(
          BOUNDARY_MATERIAL,
          (Vector2) {2.0f, SCREEN_HEIGHT_IN_METERS  - 2.0f},
          (Vector2) { 0, 0 },
          color
        )
  );
  return body;
}

frBody *CreateColoredCircle(Color color){
  frBody  *body = frCreateBodyFromShape(
      FR_BODY_DYNAMIC, 
      frVec2PixelsToMeters(GetMousePosition()),
      frCreateCircle(
        BODY_MATERIAL,
        0.1f * GetRandomValue(6, 12),
        color
      )
  );

  return body;   
}

frShape *GetTargetShape(frWorld *world, Vector2 mousePosition){
  frShape *targetShape = NULL;
  for (int i = 0; i < frGetWorldBodyCount(world); i++) {
    frBody *body = frGetWorldBody(world, i);
    frShape *shape = frGetBodyShape(body);
    
    if(CheckCollisionPointRec(frVec2PixelsToMeters(mousePosition), frGetBodyAABB(body)))
    {
      targetShape = shape;
      break;
    }
  }
  return targetShape;
}

