import { useMutation, useQueryClient } from '@tanstack/react-query'
import { z } from 'zod'

import { api } from '@/lib/api-client'
import { MutationConfig } from '@/lib/react-query'
import { Director } from '@/types/api'
import { getDirectorQueryOptions } from './get-director'

export const updateDirectorInputSchema = z.object({
  uniqueId: z.string(),
  name: z.string().min(1, 'Required')
})

export type UpdateDirectorInput = z.infer<typeof updateDirectorInputSchema>

export const updateDirector = ({
  data,
  uniqueId
}: {
  data: UpdateDirectorInput
  uniqueId: string
}): Promise<Director> => {
  return api.put(`/directors/${uniqueId}`, data)
}

type UseUpdateDirectorOptions = {
  mutationConfig?: MutationConfig<typeof updateDirector>
}

export const useUpdateDirector = ({ mutationConfig }: UseUpdateDirectorOptions = {}) => {
  const queryClient = useQueryClient()

  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (data, ...args) => {
      queryClient.refetchQueries({
        queryKey: getDirectorQueryOptions(data.uniqueId).queryKey
      })
      onSuccess?.(data, ...args)
    },
    ...restConfig,
    mutationFn: updateDirector
  })
}
