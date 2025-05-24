from fastapi import FastAPI, HTTPException, Depends
from sqlalchemy import create_engine, Column, Integer, String, Float
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, Session
from pydantic import BaseModel
from typing import List, Optional

# Configuración de la base de datos
DATABASE_URL = "mysql+mysqlconnector://root:vielmas%40salais@localhost/projecttssibackend"
engine = create_engine(DATABASE_URL)
Base = declarative_base()
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Modelo de la base de datos
class TasksProject(Base):
    __tablename__ = "TasksProject"

    Id = Column(Integer, primary_key=True, autoincrement=True)
    Nombre = Column(String(255), nullable=False)
    Tipo = Column(String(100), nullable=True)
    Prioridad = Column(Integer, nullable=True)
    QLearningPrioridad = Column(Float, nullable=True, default=0.0)
    Fecha = Column(String(255), nullable=False)

    def __repr__(self):
        return f"<TasksProject(Id={self.Id}, Nombre='{self.Nombre}', Tipo='{self.Tipo}', Prioridad={self.Prioridad}, QLearningPrioridad={self.QLearningPrioridad}, Fecha='{self.Fecha}')>"

# Crear tablas si no existen
Base.metadata.create_all(engine)

# Modelos Pydantic
class TaskBase(BaseModel):
    Nombre: str
    Tipo: Optional[str] = None
    Prioridad: Optional[int] = None
    QLearningPrioridad: Optional[float] = None
    Fecha: str

class TaskCreate(TaskBase):
    pass

class TaskUpdate(TaskBase):
    pass

class Task(TaskBase):
    Id: int

    class Config:
        orm_mode = True

# Dependencia para la sesión de la base de datos
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Instancia de la aplicación FastAPI
app = FastAPI()

# Algoritmo de Q-Learning
def update_q_learning(db: Session):
    alpha = 0.1  # Tasa de aprendizaje
    gamma = 0.9  # Factor de descuento

    tasks = db.query(TasksProject).all()
    grouped_tasks = {}
    for task in tasks:
        if task.Prioridad not in grouped_tasks:
            grouped_tasks[task.Prioridad] = []
        grouped_tasks[task.Prioridad].append(task)

    #Q-Learning por prioridad
    for priority, tasks in grouped_tasks.items():
        tasks.sort(key=lambda x: x.QLearningPrioridad, reverse=True)
        for i, task in enumerate(tasks):
            reward = len(tasks) - i
            max_q_next = max([t.QLearningPrioridad for t in tasks if t.Id != task.Id], default=0)
            task.QLearningPrioridad += alpha * (reward + gamma * max_q_next - task.QLearningPrioridad)
            db.add(task)

    db.commit()

# Rutas de la API
@app.get("/tasks", response_model=List[Task])
def get_tasks(db: Session = Depends(get_db)):
    tasks = db.query(TasksProject).all()
    return tasks

@app.get("/tasks/{task_id}", response_model=Task)
def get_task(task_id: int, db: Session = Depends(get_db)):
    task = db.query(TasksProject).filter(TasksProject.Id == task_id).first()
    if task is None:
        raise HTTPException(status_code=404, detail="Task not found")
    return task

@app.post("/tasks", response_model=Task)
def create_task(task: TaskCreate, db: Session = Depends(get_db)):
    db_task = TasksProject(**task.dict())
    db.add(db_task)
    db.commit()
    db.refresh(db_task)
    update_q_learning(db)
    return db_task

@app.put("/tasks/{task_id}", response_model=Task)
def update_task(task_id: int, task: TaskUpdate, db: Session = Depends(get_db)):
    db_task = db.query(TasksProject).filter(TasksProject.Id == task_id).first()
    if db_task is None:
        raise HTTPException(status_code=404, detail="Task not found")
    for key, value in task.dict(exclude_unset=True).items():
        setattr(db_task, key, value)
    db.commit()
    db.refresh(db_task)
    update_q_learning(db)
    return db_task

@app.delete("/tasks/{task_id}")
def delete_task(task_id: int, db: Session = Depends(get_db)):
    db_task = db.query(TasksProject).filter(TasksProject.Id == task_id).first()
    if db_task is None:
        raise HTTPException(status_code=404, detail="Task not found")
    db.delete(db_task)
    db.commit()
    update_q_learning(db)
    return {"message": "Task deleted successfully"}
