using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.Repositories;

namespace Logitude.Workflow.BL.FieldsMapping
{
    public static partial class EntityFieldsMapping
    {
        public static string GetTaskTypeName(string taskTypeId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskTypeId))
            {
                TaskTypeRepository taskTypeRepository = new TaskTypeRepository(tenant);
                TaskType taskType = taskTypeRepository.GetSingle(taskTypeId, tenant);
                return taskType?.Name;
            }
            return null;
        }

        public static string GetTaskPriorityName(string taskPriorityId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskPriorityId))
            {
                TaskPriorityRepository taskPriorityRepository = new TaskPriorityRepository(tenant);
                TaskPriority taskPriority = taskPriorityRepository.GetSingle(taskPriorityId, tenant);
                return taskPriority?.Name;
            }
            return null;
        }

        public static string GetTaskStatusName(string taskStatusId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskStatusId))
            {
                TaskStatusRepository taskStatusRepository = new TaskStatusRepository(tenant);
                TaskStatus taskStatus = taskStatusRepository.GetSingle(taskStatusId, tenant);
                return taskStatus?.Name;
            }
            return null;
        }

        public static string GetTaskFields(string taskId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                return GetTaskExtended(taskId, tenant)?.Fields;
            }
            return null;
        }

        public static string GetTaskToDoConditions(string taskId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                return GetTaskExtended(taskId, tenant)?.ToDoConditions;
            }
            return null;
        }

        public static string GetTaskDoneConditions(string taskId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                return GetTaskExtended(taskId, tenant)?.DoneConditions;
            }
            return null;
        }

        public static string GetTaskDescription(string taskId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                return GetTaskExtended(taskId, tenant)?.Description;
            }
            return null;
        }

        private static TaskExtended GetTaskExtended(string taskId, int tenant)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                TaskExtendedRepository taskExtendedRepository = new TaskExtendedRepository(tenant);
                TaskExtended taskExtended = taskExtendedRepository.GetSingle(taskId, tenant);
                return taskExtended;
            }
            return null;
        }
    }
}