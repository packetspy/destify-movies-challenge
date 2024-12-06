import { useMutation, useQueryClient } from '@tanstack/react-query'
import { z } from 'zod'

import { api } from '@/lib/api-client'
import { MutationConfig } from '@/lib/react-query'
import { Actor } from '@/types/api'
import { getActorQueryOptions } from './get-actor'

export const updateActorInputSchema = z.object({
  uniqueId: z.string(),
  name: z.string().min(1, 'Required')
})

export type UpdateActorInput = z.infer<typeof updateActorInputSchema>

export const updateActor = ({
  data,
  uniqueId
}: {
  data: UpdateActorInput
  uniqueId: string
}): Promise<Actor> => {
  return api.put(`/actors/${uniqueId}`, data)
}

type UseUpdateActorOptions = {
  mutationConfig?: MutationConfig<typeof updateActor>
}

export const useUpdateActor = ({ mutationConfig }: UseUpdateActorOptions = {}) => {
  const queryClient = useQueryClient()

  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (data, ...args) => {
      queryClient.refetchQueries({
        queryKey: getActorQueryOptions(data.uniqueId).queryKey
      })
      onSuccess?.(data, ...args)
    },
    ...restConfig,
    mutationFn: updateActor
  })
}
