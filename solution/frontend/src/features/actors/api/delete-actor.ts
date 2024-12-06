import { useMutation, useQueryClient } from '@tanstack/react-query';

import { api } from '@/lib/api-client';
import { MutationConfig } from '@/lib/react-query';

import { getActorsQueryOptions } from './get-actors';

export const deleteActor = ({
  ActorUniqueId,
}: {
  ActorUniqueId: string;
}) => {
  return api.delete(`/actors/${ActorUniqueId}`);
};

type UseDeleteActorOptions = {
  mutationConfig?: MutationConfig<typeof deleteActor>;
};

export const useDeleteActor = ({
  mutationConfig,
}: UseDeleteActorOptions = {}) => {
  const queryClient = useQueryClient();

  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: (...args) => {
      queryClient.invalidateQueries({
        queryKey: getActorsQueryOptions().queryKey,
      });
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: deleteActor,
  });
};
